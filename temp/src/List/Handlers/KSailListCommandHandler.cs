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
    if (_config.Spec.CLIOptions.ListOptions.All)
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
      return (_config.Spec.Project.Engine, _config.Spec.Project.Distribution) switch
      {
        (KSailEngine.Docker, KSailKubernetesDistribution.K3s) => await _k3dProvisioner.ListAsync(cancellationToken).ConfigureAwait(false),
        (KSailEngine.Docker, KSailKubernetesDistribution.Native) => await _kindProvisioner.ListAsync(cancellationToken).ConfigureAwait(false),
        _ => throw new NotSupportedException($"The container engine '{_config.Spec.Project.Engine}' and distribution '{_config.Spec.Project.Distribution}' combination is not supported.")
      };
    }
  }
}
