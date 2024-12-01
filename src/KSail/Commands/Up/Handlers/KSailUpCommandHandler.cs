using System.Runtime.InteropServices;
using System.Text;
using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KeyManager.Local.Age;
using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using Devantler.KubernetesProvisioner.GitOps.Flux;
using Devantler.KubernetesProvisioner.Resources.Native;
using k8s;
using k8s.Models;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler
{
  //TODO: readonly CiliumProvisioner _cniProvisioner = new();
  readonly LocalAgeKeyManager _keyManager = new();
  readonly DockerProvisioner _containerEngineProvisioner;
  readonly FluxProvisioner _gitOpsProvisioner;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;
  readonly KSailCluster _config;
  readonly KSailDownCommandHandler _ksailDownCommandHandler;
  readonly KSailLintCommandHandler _ksailLintCommandHandler = new();

  internal KSailUpCommandHandler(KSailCluster config)
  {
    _containerEngineProvisioner = config.Spec.Project.ContainerEngine switch
    {
      KSailContainerEngine.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"The container engine '{config.Spec.Project.ContainerEngine}' is not supported.")
    };
    _clusterProvisioner = config.Spec.Project.Distribution switch
    {
      KSailKubernetesDistribution.K3d => new K3dProvisioner(),
      KSailKubernetesDistribution.Kind => new KindProvisioner(),
      _ => throw new NotSupportedException($"The distribution '{config.Spec.Project.Distribution}' is not supported.")
    };
    _gitOpsProvisioner = config.Spec.Project.GitOpsTool switch
    {
      KSailGitOpsTool.Flux => new FluxProvisioner(config.Spec.Connection.Context),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec.Project.GitOpsTool}' is not supported.")
    };
    _ksailDownCommandHandler = new KSailDownCommandHandler(config);
    _config = config;
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {

    if (!await CheckContainerEngineIsRunning(cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    await CreateRegistries(_config, cancellationToken).ConfigureAwait(false);

    if (!await Lint(_config, cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    if (!await DestroyExistingCluster(cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    await ProvisionCluster(cancellationToken).ConfigureAwait(false);

    await InstallGitOps(_config, cancellationToken).ConfigureAwait(false);

    if (_config.Spec.CLI.UpOptions.Reconcile)
    {
      Console.WriteLine("🔄 Reconciling kustomizations");
      await _gitOpsProvisioner.ReconcileAsync(_config.Spec.Project.KustomizeFlows.Reverse().ToArray(), _config.Spec.Connection.Timeout, cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
    }
    return 0;
  }

  async Task<bool> DestroyExistingCluster(CancellationToken cancellationToken)
  {
    if (_config.Spec.CLI.UpOptions.Destroy)
    {
      Console.WriteLine($"🔥 Destroying existing cluster '{_config.Metadata.Name}'");
      bool success = await _ksailDownCommandHandler.HandleAsync(cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
      return success;
    }
    return true;
  }

  async Task<bool> CheckContainerEngineIsRunning(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🐳 Checking {_config.Spec.Project.ContainerEngine} is running");
    if (await _containerEngineProvisioner.CheckReadyAsync(cancellationToken).ConfigureAwait(false))
    {
      Console.WriteLine($"✔ {_config.Spec.Project.ContainerEngine} is running");
      Console.WriteLine();
      return true;
    }
    else
    {
      Console.WriteLine($"✗ {_config.Spec.Project.ContainerEngine} is not running");
      Console.WriteLine();
      return false;
    }
  }

  async Task CreateRegistries(KSailCluster config, CancellationToken cancellationToken = default)
  {
    Console.WriteLine("🧮 Creating registries");
    foreach (var registry in config.Spec.Registries ?? [])
    {
      if (registry.IsGitOpsOCISource)
      {
        Console.WriteLine($"► creating registry '{registry.Name}' on port '{registry.HostPort}' for GitOps OCI source");
      }
      else if (registry.Proxy is null)
      {
        Console.WriteLine($"► creating registry '{registry.Name}' on port '{registry.HostPort}'");
      }
      else
      {
        Console.WriteLine($"► creating mirror registry '{registry.Name}' on port '{registry.HostPort}' for '{registry?.Proxy?.Url}'");
      }
      var proxyUrl = registry?.Proxy?.Url;
      await _containerEngineProvisioner
       .CreateRegistryAsync(registry!.Name, registry.HostPort, proxyUrl, cancellationToken).ConfigureAwait(false);
    }
    Console.WriteLine();
  }

  async Task<bool> Lint(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.CLI.UpOptions.Lint)
    {
      Console.WriteLine("🔍 Linting manifests");
      bool success = await _ksailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
      return success;
    }
    return true;
  }

  async Task ProvisionCluster(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🚀 Provisioning cluster '{_config.Spec.Project.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{_config.Metadata.Name}'");
    await _clusterProvisioner.ProvisionAsync(_config.Metadata.Name, _config.Spec.Project.ConfigPath, cancellationToken).ConfigureAwait(false);
    Console.WriteLine();
  }

  async Task InstallGitOps(KSailCluster config, CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🔼 Bootstrapping GitOps with {config.Spec.Project.GitOpsTool}");
    Console.WriteLine("► creating 'flux-system' namespace");
    using var resourceProvisioner = new KubernetesResourceProvisioner(config.Spec.Connection.Context);
    _ = await resourceProvisioner.CreateNamespaceAsync(new V1Namespace
    {
      Metadata = new V1ObjectMeta
      {
        Name = "flux-system"
      }
    }, cancellationToken: cancellationToken).ConfigureAwait(false);

    await InitializeSOPSAgeSecret(config, resourceProvisioner, cancellationToken).ConfigureAwait(false);
    string ociUrlOnHost = $"oci://localhost:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}";
    await _gitOpsProvisioner.PushManifestsAsync(new Uri(ociUrlOnHost), config.Spec.Project.ManifestsDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
    string kustomizationDirectoryInOCI = config.Spec.Project.KustomizationDirectory.Replace("k8s/", "", StringComparison.OrdinalIgnoreCase);
    string ociUrlInDocker = _config.Spec.Project.Distribution switch
    {
      KSailKubernetesDistribution.K3d => $"oci://host.k3d.internal:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}",
      KSailKubernetesDistribution.Kind =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
          $"oci://172.17.0.1:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}" :
          $"oci://host.docker.internal:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}",
      _ => throw new NotSupportedException($"The distribution '{_config.Spec.Project.Distribution}' is not supported.")
    };
    await _gitOpsProvisioner.BootstrapAsync(new Uri(ociUrlInDocker), kustomizationDirectoryInOCI, true, cancellationToken).ConfigureAwait(false);
    Console.WriteLine();
  }

  async Task InitializeSOPSAgeSecret(KSailCluster config, KubernetesResourceProvisioner resourceProvisioner, CancellationToken cancellationToken)
  {
    if (config.Spec.Project.Sops)
    {
      Console.WriteLine("► searching for a '.sops.yaml' file");
      string directory = Directory.GetCurrentDirectory();
      string sopsConfigPath = string.Empty;
      while (!string.IsNullOrEmpty(directory))
      {
        if (File.Exists(Path.Combine(directory, ".sops.yaml")))
        {
          sopsConfigPath = Path.Combine(directory, ".sops.yaml");
          Console.WriteLine($"✔ found '{sopsConfigPath}'");
          break;
        }
        directory = Directory.GetParent(directory)?.FullName ?? string.Empty;
      }
      if (string.IsNullOrEmpty(sopsConfigPath))
      {
        Console.WriteLine("✗ '.sops.yaml' file not found");
        throw new KSailException("No '.sops.yaml' file found in the current or parent directories");
      }

      Console.WriteLine("► reading public key from '.sops.yaml' file");
      var sopsConfig = await _keyManager.GetSOPSConfigAsync(sopsConfigPath, cancellationToken).ConfigureAwait(false);
      string publicKey = sopsConfig.CreationRules.First(x => x.PathRegex.Contains(config.Metadata.Name, StringComparison.OrdinalIgnoreCase)).Age.Split(',')[0].Trim();

      Console.WriteLine("► getting private key from SOPS_AGE_KEY_FILE or default location");
      var ageKey = await _keyManager.GetKeyAsync(publicKey, cancellationToken).ConfigureAwait(false);

      Console.WriteLine("► creating 'sops-age' secret in 'flux-system' namespace");
      var secret = new V1Secret
      {
        Metadata = new V1ObjectMeta
        {
          Name = "sops-age",
          NamespaceProperty = "flux-system"
        },
        Type = "Generic",
        Data = new Dictionary<string, byte[]>
        {
          { "sops.agekey", Encoding.UTF8.GetBytes(ageKey.PrivateKey) }
        }
      };
      _ = await resourceProvisioner.CreateNamespacedSecretAsync(secret, secret.Metadata.NamespaceProperty, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
  }
}
