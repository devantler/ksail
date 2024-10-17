using System.CommandLine;
using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Options;

sealed class DistributionOption()
 : Option<KSailKubernetesDistribution?>(
    ["-d", "--distribution"],
    "The distribution to use for the cluster."
  )
{
}
