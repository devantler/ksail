using Devantler.KubernetesProvisioner.Cluster.Core;

namespace KSail.Commands.Start.Handlers;

class KSailStartCommandHandler(IKubernetesClusterProvisioner clusterProvisioner)
{
  readonly IKubernetesClusterProvisioner _clusterProvisioner = clusterProvisioner;
  internal async Task HandleAsync(string clusterName, CancellationToken cancellationToken) => await _clusterProvisioner.StartAsync(clusterName, cancellationToken).ConfigureAwait(false);
}
