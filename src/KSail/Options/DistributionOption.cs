using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

sealed class ProjectDistributionOption()
 : Option<KSailKubernetesDistribution>(
    ["-d", "--distribution"],
    "The distribution to use for the cluster."
  )
{
}
