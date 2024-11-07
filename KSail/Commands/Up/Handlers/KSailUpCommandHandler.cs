using Devantler.ContainerEngineProvisioner.Docker;
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

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler
{
  //TODO: readonly CiliumProvisioner _cniProvisioner = new();
  //TODO: readonly LocalAgeKeyManager _keyManager = new();
  readonly DockerProvisioner _containerEngineProvisioner;
  readonly FluxProvisioner _gitOpsProvisioner;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;
  readonly KSailCluster _config;
  readonly KSailDownCommandHandler _ksailDownCommandHandler;
  readonly KSailLintCommandHandler _ksailLintCommandHandler = new();

  internal KSailUpCommandHandler(KSailCluster config)
  {
    _containerEngineProvisioner = config.Spec.ContainerEngine switch
    {
      KSailContainerEngine.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"The container engine '{config.Spec.ContainerEngine}' is not supported.")
    };
    _clusterProvisioner = config.Spec.Distribution switch
    {
      KSailKubernetesDistribution.K3d => new K3dProvisioner(),
      KSailKubernetesDistribution.Kind => new KindProvisioner(),
      _ => throw new NotSupportedException($"The distribution '{config.Spec.Distribution}' is not supported.")
    };
    _gitOpsProvisioner = config.Spec.GitOpsTool switch
    {
      KSailGitOpsTool.Flux => new FluxProvisioner(config.Spec.Context),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec.GitOpsTool}' is not supported.")
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
    return 0;
  }

  async Task<bool> DestroyExistingCluster(CancellationToken cancellationToken)
  {
    if (_config.Spec.UpOptions.Destroy)
    {
      Console.WriteLine($"🔥 Destroying existing cluster '{_config.Metadata.Name}'");
      bool success = await _ksailDownCommandHandler.HandleAsync(cancellationToken).ConfigureAwait(false);
      Console.WriteLine("");
      return success;
    }
    return true;
  }

  async Task<bool> CheckContainerEngineIsRunning(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🐳 Checking {_config.Spec.ContainerEngine} is running");
    if (await _containerEngineProvisioner.CheckReadyAsync(cancellationToken).ConfigureAwait(false))
    {
      Console.WriteLine($"✔ {_config.Spec.ContainerEngine} is running");
      Console.WriteLine("");
      return true;
    }
    else
    {
      Console.WriteLine($"✗ {_config.Spec.ContainerEngine} is not running");
      Console.WriteLine("");
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
        Console.WriteLine($"► Creating registry '{registry.Name}' on port '{registry.HostPort}' for GitOps OCI source");
      }
      else if (registry.Proxy is null)
      {
        Console.WriteLine($"► Creating registry '{registry.Name}' on port '{registry.HostPort}'");
      }
      else
      {
        Console.WriteLine($"► Creating mirror registry '{registry.Name}' on port '{registry.HostPort}' for '{registry?.Proxy?.Url}'");
      }
      var proxyUrl = registry?.Proxy?.Url;
      await _containerEngineProvisioner
       .CreateRegistryAsync(registry!.Name, registry.HostPort, proxyUrl, cancellationToken).ConfigureAwait(false);
    }
    Console.WriteLine("");
  }

  async Task<bool> Lint(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.UpOptions.Lint)
    {
      Console.WriteLine("🔍 Linting manifests");
      bool success = await _ksailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
      Console.WriteLine("");
      return success;
    }
    return true;
  }

  async Task ProvisionCluster(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🚀 Provisioning cluster '{_config.Metadata.Name}'");
    await _clusterProvisioner.ProvisionAsync(_config.Metadata.Name, _config.Spec.ConfigPath, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("");
  }

  async Task InstallGitOps(KSailCluster config, CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🔼 Bootstrapping GitOps with {config.Spec.GitOpsTool}");
    Console.WriteLine("► Creating 'flux-system' namespace");
    using var resourceProvisioner = new KubernetesResourceProvisioner(config.Spec.Context);
    _ = await resourceProvisioner.CreateNamespaceAsync(new V1Namespace
    {
      Metadata = new V1ObjectMeta
      {
        Name = "flux-system"
      }
    }, cancellationToken: cancellationToken).ConfigureAwait(false);

    if (config.Spec.Sops)
    {
      // //TODO: Check that a .sops.yaml file exists in the current directory or a parent directory.
      // Console.WriteLine("► Searching for a '.sops.yaml' file in the current directory or a parent directory");

      // Console.WriteLine("✗ '.sops.yaml' file not found");
      // //TODO: Read the public key from the .sops.yaml file, for the specified cluster. The path_regex should contain the cluster name.
      // Console.WriteLine("► Creating 'sops-age' secret");
      // Console.WriteLine("");
      // //TODO: Get public key from the .sops.yaml file.
      // var key = _keyManager.GetKeyAsync("public-key", cancellationToken);
      // using var sopsProvisioner = new LocalProvisioner();
      // if (await sopsProvisioner.ProvisionAsync(KeyType.Age, clusterName, context, cancellationToken).ConfigureAwait(false) != 0)
      // {
      //   Console.WriteLine(ResourceManager.GetString("flux-install-sops-provision-failed", CultureInfo.InvariantCulture));
      //   return 1;
      // }
      // Console.WriteLine(ResourceManager.GetString("flux-install-sops-provision-success", CultureInfo.InvariantCulture));
      // Console.WriteLine("");
    }
    string ociUrlOnHost = $"oci://localhost:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}";
    Console.WriteLine($"► Pushing '{config.Spec.ManifestsDirectory}' as an OCI Artifact to '{ociUrlOnHost}'");
    await _gitOpsProvisioner.PushManifestsAsync(new Uri(ociUrlOnHost), config.Spec.ManifestsDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
    string kustomizationDirectoryInOCI = config.Spec.KustomizationDirectory.Replace("k8s/", "", StringComparison.OrdinalIgnoreCase);
    string ociUrlInDocker = _config.Spec.Distribution switch
    {
      KSailKubernetesDistribution.K3d => $"oci://host.k3d.internal:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}",
      KSailKubernetesDistribution.Kind => $"oci://host.docker.internal:{_config.Spec.Registries.First(x => x.IsGitOpsOCISource).HostPort}/{_config.Metadata.Name}",
      _ => throw new NotSupportedException($"The distribution '{_config.Spec.Distribution}' is not supported.")
    };
    await _gitOpsProvisioner.BootstrapAsync(new Uri(ociUrlInDocker), kustomizationDirectoryInOCI, true, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("");

    if (config.Spec.UpOptions.Reconcile)
    {
      Console.WriteLine("🔄 Reconciling kustomizations");
      await _gitOpsProvisioner.ReconcileAsync(_config.Spec.Timeout, cancellationToken).ConfigureAwait(false);
      Console.WriteLine("");
    }
  }
}
