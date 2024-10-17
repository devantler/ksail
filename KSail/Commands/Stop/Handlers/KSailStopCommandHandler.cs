using Devantler.KubernetesProvisioner.Cluster.Core;

namespace KSail.Commands.Stop.Handlers;

class KSailStopCommandHandler(IKubernetesClusterProvisioner clusterProvisioner)
{
  readonly IKubernetesClusterProvisioner _clusterProvisioner = clusterProvisioner;
  internal Task HandleAsync(string clusterName, CancellationToken cancellationToken) => _clusterProvisioner.StopAsync(clusterName, cancellationToken);
}
