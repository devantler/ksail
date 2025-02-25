using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.Cluster.Kind;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Commands.Start.Handlers;

class KSailStartCommandHandler
{
  readonly KSailCluster _config;
  readonly IKubernetesClusterProvisioner _clusterProvisioner;

  internal KSailStartCommandHandler(KSailCluster config)
  {
    _config = config;
    _clusterProvisioner = (_config.Spec.Project.Engine, _config.Spec.Project.Distribution) switch
    {
      (KSailEngineType.Docker, KSailKubernetesDistributionType.Native) => new KindProvisioner(),
      (KSailEngineType.Docker, KSailKubernetesDistributionType.K3s) => new K3dProvisioner(),
      _ => throw new NotSupportedException($"The engine '{_config.Spec.Project.Engine}' and distribution '{_config.Spec.Project.Distribution}' combination is not supported")
    };
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"► starting cluster '{_config.Spec.Connection.Context}'");
    await _clusterProvisioner.StartAsync(_config.Metadata.Name, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ cluster started");
    return 0;
  }
}
