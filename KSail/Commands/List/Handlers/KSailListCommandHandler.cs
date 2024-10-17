using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler()
{
  readonly K3dProvisioner _k3dProvisioner = new();
  readonly KindProvisioner _kindProvisioner = new();

  internal async Task<IEnumerable<string>> HandleAsync(CancellationToken cancellationToken)
  {
    var clusters = await _k3dProvisioner.ListAsync(cancellationToken).ConfigureAwait(false);
    clusters = clusters.Concat(await _kindProvisioner.ListAsync(cancellationToken).ConfigureAwait(false)).ToArray();
    return clusters;
  }
}
