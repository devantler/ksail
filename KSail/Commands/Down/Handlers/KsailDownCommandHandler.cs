using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using KSail.Models;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler
{
  readonly KSailCluster _config;
  readonly DockerProvisioner _containerEngineProvisioner;
  readonly IKubernetesClusterProvisioner _kubernetesDistributionProvisioner;

  internal KSailDownCommandHandler(KSailCluster config)
  {
    _config = config;
    _containerEngineProvisioner = _config.Spec.ContainerEngine switch
    {
      KSailContainerEngine.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"Container engine '{_config.Spec.ContainerEngine}' is not supported.")
    };
    _kubernetesDistributionProvisioner = _config.Spec.Distribution switch
    {
      KSailKubernetesDistribution.K3d => new K3dProvisioner(),
      KSailKubernetesDistribution.Kind => new KindProvisioner(),
      _ => throw new NotSupportedException($"Kubernetes distribution '{_config.Spec.ContainerEngine}' is not supported.")
    };
  }

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    await _kubernetesDistributionProvisioner.DeprovisionAsync(_config.Metadata.Name, cancellationToken).ConfigureAwait(false);
    if (_config.Spec.DownOptions.Registries)
    {
      Console.WriteLine("► deleting registries...");
      await DeleteRegistriesAsync(cancellationToken).ConfigureAwait(false);
    }
    return true;
  }

  async Task DeleteRegistriesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var registry in _config.Spec.Registries)
    {
      await _containerEngineProvisioner.DeleteRegistryAsync(registry.Name, cancellationToken).ConfigureAwait(false);
    }
  }
}
