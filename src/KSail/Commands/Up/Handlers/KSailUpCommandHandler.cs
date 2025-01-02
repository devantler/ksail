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
using KSail.Models.DeploymentTool;
using KSail.Models.Project;
using KSail.Utils;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler
{
  //TODO: readonly CiliumProvisioner _cniProvisioner = new();
  readonly LocalAgeKeyManager _keyManager = new();
  readonly DockerProvisioner _engineProvisioner;
  readonly FluxProvisioner _deploymentTool;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;
  readonly KSailCluster _config;
  readonly KSailDownCommandHandler _ksailDownCommandHandler;
  readonly KSailLintCommandHandler _ksailLintCommandHandler = new();

  internal KSailUpCommandHandler(KSailCluster config)
  {
    _engineProvisioner = config.Spec.Project.Engine switch
    {
      KSailEngine.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"The container engine '{config.Spec.Project.Engine}' is not supported.")
    };
    _clusterProvisioner = (config.Spec.Project.Engine, config.Spec.Project.Distribution) switch
    {
      (KSailEngine.Docker, KSailKubernetesDistribution.Native) => new KindProvisioner(),
      (KSailEngine.Docker, KSailKubernetesDistribution.K3s) => new K3dProvisioner(),
      _ => throw new NotSupportedException($"The distribution '{config.Spec.Project.Distribution}' is not supported.")
    };
    _deploymentTool = config.Spec.Project.DeploymentTool switch
    {
      KSailDeploymentTool.Flux => new FluxProvisioner(config.Spec.Connection.Context),
      _ => throw new NotSupportedException($"The Deployment tool '{config.Spec.Project.DeploymentTool}' is not supported.")
    };
    _ksailDownCommandHandler = new KSailDownCommandHandler(config);
    _config = config;
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {

    if (!await CheckEngineIsRunning(cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    await CreatePrerequisites(_config, cancellationToken).ConfigureAwait(false);

    if (!await Lint(_config, cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    if (!await DestroyExistingCluster(cancellationToken).ConfigureAwait(false))
    {
      return 1;
    }

    await ProvisionCluster(cancellationToken).ConfigureAwait(false);

    await BootstrapSecretManager(_config, cancellationToken).ConfigureAwait(false);

    await BootstrapDeploymentTool(_config, cancellationToken).ConfigureAwait(false);

    if (_config.Spec.CLIOptions.UpOptions.Reconcile)
    {
      Console.WriteLine("🔄 Reconciling kustomizations");
      await _deploymentTool.ReconcileAsync(_config.Spec.KustomizeTemplateOptions.Kustomizations.Reverse().ToArray(), _config.Spec.Connection.Timeout, cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
    }
    return 0;
  }

  async Task<bool> DestroyExistingCluster(CancellationToken cancellationToken)
  {
    if (_config.Spec.CLIOptions.UpOptions.Destroy)
    {
      Console.WriteLine($"🔥 Destroying existing cluster '{_config.Metadata.Name}'");
      bool success = await _ksailDownCommandHandler.HandleAsync(cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
      return success;
    }
    return true;
  }

  async Task<bool> CheckEngineIsRunning(CancellationToken cancellationToken = default)
  {
    string engineEmoji = _config.Spec.Project.Engine switch
    {
      KSailEngine.Docker => "🐳",
      _ => throw new NotSupportedException($"The container engine '{_config.Spec.Project.Engine}' is not supported.")
    };
    Console.WriteLine($"{engineEmoji} Checking {_config.Spec.Project.Engine} is running");
    if (await _engineProvisioner.CheckReadyAsync(cancellationToken).ConfigureAwait(false))
    {
      Console.WriteLine($"✔ {_config.Spec.Project.Engine} is running");
      Console.WriteLine();
      return true;
    }
    else
    {
      Console.WriteLine($"✗ {_config.Spec.Project.Engine} is not running");
      Console.WriteLine();
      return false;
    }
  }

  async Task CreatePrerequisites(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.Project.Engine == KSailEngine.Docker && config.Spec.Project.DeploymentTool == KSailDeploymentTool.Flux)
    {
      if (config.Spec.FluxDeploymentToolOptions.Source is KSailOCIRepository)
      {
        Console.WriteLine("📥 Create OCI source registry");
        int port = config.Spec.FluxDeploymentToolOptions.Source.Url.Port;
        Console.WriteLine($"► creating '{config.Spec.FluxDeploymentToolOptions.Source.Url}' as Flux OCI source registry");
        await _engineProvisioner
         .CreateRegistryAsync(
          config.Spec.FluxDeploymentToolOptions.Source.Url.Segments.Last(),
          port,
          cancellationToken: cancellationToken
        ).ConfigureAwait(false);

        Console.WriteLine();
      }
    }
    Console.WriteLine("🧮 Creating mirror registries");
    // foreach (var mirrorRegistry in config.Spec.MirrorRegistryOptions.MirrorRegistries)
    // {
    //   Console.WriteLine($"► creating mirror registry '{mirrorRegistry.Name} for '{mirrorRegistry.Proxy.Url}'");
    //   var proxyUrl = mirrorRegistry.Proxy?.Url;
    //   await _engineProvisioner
    //    .CreateRegistryAsync(mirrorRegistry!.Name, mirrorRegistry.HostPort, proxyUrl, cancellationToken).ConfigureAwait(false);
    // }
    Console.WriteLine();
  }

  async Task<bool> Lint(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.CLIOptions.UpOptions.Lint)
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

  async Task BootstrapDeploymentTool(KSailCluster config, CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"🔼 Bootstrapping GitOps with {config.Spec.Project.DeploymentTool}");
    Console.WriteLine("► creating 'flux-system' namespace");
    using var resourceProvisioner = new KubernetesResourceProvisioner(config.Spec.Connection.Context);
    _ = await resourceProvisioner.CreateNamespaceAsync(new V1Namespace
    {
      Metadata = new V1ObjectMeta
      {
        Name = "flux-system"
      }
    }, cancellationToken: cancellationToken).ConfigureAwait(false);

    await _deploymentTool.PushManifestsAsync(_config.Spec.FluxDeploymentToolOptions.Source.Url, "k8s", cancellationToken: cancellationToken).ConfigureAwait(false);
    await _deploymentTool.BootstrapAsync(config.Spec.FluxDeploymentToolOptions.Source.Url,
      config.Spec.KustomizeTemplateOptions.RootKustomization.Replace("k8s/", "", StringComparison.OrdinalIgnoreCase),
      true,
      cancellationToken
    ).ConfigureAwait(false);
    Console.WriteLine();
  }

  async Task BootstrapSecretManager(KSailCluster config, CancellationToken cancellationToken)
  {
    using var resourceProvisioner = new KubernetesResourceProvisioner(config.Spec.Connection.Context);
    if (config.Spec.Project.SecretManager == KSailSecretManager.SOPS)
    {
      var sopsConfig = await SopsConfigLoader.LoadAsync(cancellationToken).ConfigureAwait(false);

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
