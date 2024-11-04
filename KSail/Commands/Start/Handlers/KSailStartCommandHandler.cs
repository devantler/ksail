using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using KSail.Models;

namespace KSail.Commands.Start.Handlers;

class KSailStartCommandHandler
{
  readonly KSailCluster _config;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;

  internal KSailStartCommandHandler(KSailCluster config)
  {
    _config = config;
    _clusterProvisioner = _config.Spec.Distribution switch
    {
      KSailKubernetesDistribution.K3d => new K3dProvisioner(),
      KSailKubernetesDistribution.Kind => new KindProvisioner(),
      _ => throw new NotSupportedException($"The distribution '{_config.Spec.Distribution}' is not supported.")
    };
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {
    await _clusterProvisioner.StartAsync(_config.Metadata.Name, cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
