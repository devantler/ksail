using System.Globalization;
using System.Resources;
using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KeyManager.Local.Age;
using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using Devantler.KubernetesProvisioner.GitOps.Core;
using Devantler.KubernetesProvisioner.GitOps.Flux;
using Devantler.KubernetesProvisioner.Resources.Native;
using k8s;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Models;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler : IDisposable
{
  readonly DockerProvisioner _containerEngineProvisioner;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner;
  readonly KubernetesResourceProvisioner _resourceProvisioner;
  readonly KSailUpdateCommandHandler _ksailUpdateCommandHandler;
  readonly LocalAgeKeyManager _keyManager = new();
  readonly KSailCluster _config;

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
      KSailGitOpsTool.Flux => new FluxProvisioner(),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec.GitOpsTool}' is not supported.")
    };
    _resourceProvisioner = new KubernetesResourceProvisioner(config.Spec.Context);
    _ksailUpdateCommandHandler = new KSailUpdateCommandHandler(config);
    _config = config;
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"🐳 Checking {_config.Spec.ContainerEngine} is running");
    if (await CheckContainerEngineIsUp(cancellationToken).ConfigureAwait(false))
    {
      Console.WriteLine($"✔ {_config.Spec.ContainerEngine} is running");
    }
    else
    {
      throw new KSailException($"{_config.Spec.ContainerEngine} is not running");
    }
    Console.WriteLine("");

    await CreateRegistries(config, cancellationToken).ConfigureAwait(false);
    await KSailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
    await ProvisionCluster(config, cancellationToken).ConfigureAwait(false);

    await InstallFlux(config, cancellationToken).ConfigureAwait(false);

    await _ksailUpdateCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
    return 0;

  }

  async Task<bool> CheckContainerEngineIsUp(CancellationToken cancellationToken) =>
    await _containerEngineProvisioner.CheckReadyAsync(cancellationToken).ConfigureAwait(false);

  async Task CreateRegistries(KSailCluster config, CancellationToken cancellationToken)
  {
    Console.WriteLine("🧮 Creating registries");
    foreach (var registry in config.Spec.Registries ?? [])
    {
      _containerEngineProvisioner.CreateRegistryAsync(registry.Name, registry.HostPort, cancellationToken);
    }
    if (await CreatePullThroughRegistry("docker.io", 5001, new Uri("https://registry-1.docker.io"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("registry.k8s.io", 5002, new Uri("https://registry.k8s.io"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("gcr.io", 5003, new Uri("https://gcr.io"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("ghcr.io", 5004, new Uri("https://ghcr.io"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("quay.io", 5005, new Uri("https://quay.io"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"), cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    if (await CreatePullThroughRegistry("manifests", 5050, null, cancellationToken).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    Console.WriteLine();
    return 0;
  }

  async Task ProvisionCluster(KSailCluster config, CancellationToken cancellationToken)
  {
    Console.WriteLine($"🚀 Provisioning cluster '{config.Metadata.Name}'");
    await _clusterProvisioner.ProvisionAsync(config.Metadata.Name, config.Spec.ConfigPath, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("");
  }

  async Task InstallFlux(KSailCluster config, CancellationToken cancellationToken)
  {
    Console.WriteLine("🔼 Installing Flux");
    Console.WriteLine("► Creating 'flux-system' namespace");

#pragma warning disable CA1308 // Normalize strings to uppercase
    string context = $"{config.Spec.Distribution.ToString().ToLowerInvariant()}-{config.Metadata.Name}";
#pragma warning restore CA1308 // Normalize strings to uppercase
    await _resourceProvisioner.CreateNamespaceAsync(context, "flux-system").ConfigureAwait(false);
    Console.WriteLine("");

    if (config.Spec.Sops == true)
    {
      //TODO: Check that a .sops.yaml file exists in the current directory or a parent directory.
      Console.WriteLine("► Searching for a '.sops.yaml' file in the current directory or a parent directory");

      Console.WriteLine("✕ '.sops.yaml' file not found");
      //TODO: Read the public key from the .sops.yaml file, for the specified cluster. The path_regex should contain the cluster name.
      Console.WriteLine("► Creating 'sops-age' secret");
      Console.WriteLine("");
      //TODO: Get public key from the .sops.yaml file.
      var key = _keyManager.GetKeyAsync("public-key", cancellationToken);
      using var sopsProvisioner = new LocalProvisioner();
      if (await sopsProvisioner.ProvisionAsync(KeyType.Age, clusterName, context, cancellationToken).ConfigureAwait(false) != 0)
      {
        Console.WriteLine(ResourceManager.GetString("flux-install-sops-provision-failed", CultureInfo.InvariantCulture));
        return 1;
      }
      Console.WriteLine(ResourceManager.GetString("flux-install-sops-provision-success", CultureInfo.InvariantCulture));
      Console.WriteLine("");
    }
    var kubernetesDistribution = await _clusterProvisioner.GetKubernetesDistributionTypeAsync().ConfigureAwait(false);
#pragma warning disable CA1308 // Normalize strings to uppercase
    string k8sContext = $"{kubernetesDistribution.ToString().ToLowerInvariant()}-{clusterName}";
#pragma warning restore CA1308 // Normalize strings to uppercase
    string ociUrl = $"oci://host.k3d.internal:5050/{clusterName}";

    if (await _gitOpsProvisioner.InstallAsync(k8sContext, ociUrl, kustomizationsPath, cancellationToken).ConfigureAwait(false) != 0)
    {
      Console.WriteLine(ResourceManager.GetString("flux-install-failed", CultureInfo.InvariantCulture));
      return 1;
    }
    Console.WriteLine("");

    Console.WriteLine(ResourceManager.GetString("flux-reconcile", CultureInfo.InvariantCulture));
    return await new KSailCheckCommandHandler().HandleAsync(context, timeout, cancellationToken).ConfigureAwait(false) != 0 ? 1 : 0;
  }

  async Task<int> CreatePullThroughRegistry(string name, int port, Uri? url = default, CancellationToken cancellationToken = default)
  {
    if (url is null)
    {
      Console.WriteLine($"► Creating pull-through registry '{name}' on port '{port}'");
    }
    else
    {
      Console.WriteLine($"► Creating pull-through registry '{name}' on port '{port}' for '{url}'");
    }
    try
    {
      await _containerEngineProvisioner.CreateRegistryAsync(name, port, url, cancellationToken).ConfigureAwait(false);
    }
    catch (ContainerEngineException e)
    {
      Console.WriteLine($"✕ Failed to create pull-through registry '{name}': {e.Message}");
      return 1;
    }
    Console.WriteLine($"✔ Pull-through registry '{name}' created");
    return 0;
  }

  public void Dispose() => _resourceProvisioner.Dispose();
}
