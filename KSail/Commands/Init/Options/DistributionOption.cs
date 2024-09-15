using System.CommandLine;
using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Commands.Init.Options;

sealed class DistributionOption()
 : Option<KSailKubernetesDistribution>(
    ["-d", "--distribution"],
    () => KSailKubernetesDistribution.K3d,
    "The distribution to use for the cluster."
  )
{
}
