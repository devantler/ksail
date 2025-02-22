using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

/// <summary>
/// The distribution to use for the cluster.
/// </summary>
public class ProjectDistributionOption()
 : Option<KSailKubernetesDistribution>(
    ["-d", "--distribution"],
    "The distribution to use for the cluster."
  )
{
}
