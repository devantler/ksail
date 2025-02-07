using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler
{
  readonly KSailCluster _config;
  readonly DockerProvisioner _engineProvisioner;
  readonly IKubernetesClusterProvisioner _kubernetesDistributionProvisioner;

  internal KSailDownCommandHandler(KSailCluster config)
  {
    _config = config;
    _engineProvisioner = _config.Spec.Project.Engine switch
    {
      KSailEngine.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"Engine '{_config.Spec.Project.Engine}' is not supported.")
    };
    _kubernetesDistributionProvisioner = _config.Spec.Project.Distribution switch
    {
      KSailKubernetesDistribution.K3s => new K3dProvisioner(),
      KSailKubernetesDistribution.Native => new KindProvisioner(),
      _ => throw new NotSupportedException($"Kubernetes distribution '{_config.Spec.Project.Engine}' is not supported.")
    };
  }

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    await _kubernetesDistributionProvisioner.DeleteAsync(_config.Metadata.Name, cancellationToken).ConfigureAwait(false);
    if (_config.Spec.CLIOptions.DownOptions.Registries)
    {
      Console.WriteLine("► deleting registries...");
      await DeleteRegistriesAsync(cancellationToken).ConfigureAwait(false);
    }
    return true;
  }

  async Task DeleteRegistriesAsync(CancellationToken cancellationToken = default)
  {
    switch (_config.Spec.Project.DeploymentTool)
    {
      case KSailDeploymentTool.Flux:
        await _engineProvisioner.DeleteRegistryAsync(_config.Spec.FluxDeploymentToolOptions.Source.Url.Segments.Last(), cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"deployment tool '{_config.Spec.Project.DeploymentTool}' is not supported.");
    }
    if (_config.Spec.Project.MirrorRegistries)
    {
      foreach (var registry in _config.Spec.MirrorRegistryOptions.MirrorRegistries)
      {
        await _engineProvisioner.DeleteRegistryAsync(registry.Name, cancellationToken).ConfigureAwait(false);
      }
    }
  }
}
