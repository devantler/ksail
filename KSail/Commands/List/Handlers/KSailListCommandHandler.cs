using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler(KSailCluster config)
{
  readonly KSailCluster _config = config;
  readonly K3dProvisioner _k3dProvisioner = new();
  readonly KindProvisioner _kindProvisioner = new();

  internal async Task<IEnumerable<string>> HandleAsync(CancellationToken cancellationToken = default)
  {
    if (_config.Spec.CLI.ListOptions.All)
    {
      IEnumerable<string> clusters = [];
      Console.WriteLine("---- K3d ----");
      clusters = clusters.Concat(await _k3dProvisioner.ListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

      Console.WriteLine();

      Console.WriteLine("---- Kind ----");
      clusters = clusters.Concat(await _kindProvisioner.ListAsync(cancellationToken).ConfigureAwait(false)).ToArray();
      return clusters;
    }
    else
    {
      return _config.Spec.Project.Distribution switch
      {
        KSailKubernetesDistribution.K3d => await _k3dProvisioner.ListAsync(cancellationToken).ConfigureAwait(false),
        KSailKubernetesDistribution.Kind => await _kindProvisioner.ListAsync(cancellationToken).ConfigureAwait(false),
        _ => throw new NotSupportedException($"Distribution {_config.Spec.Project.Distribution} is not supported."),
      };
    }
  }
}
