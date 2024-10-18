using System.CommandLine;
using KSail.Models;

namespace KSail.Options;

sealed class DistributionOption()
 : Option<KSailKubernetesDistribution?>(
    ["-d", "--distribution"],
    () => KSailKubernetesDistribution.Kind,
    "The distribution to use for the cluster."
  )
{
}
