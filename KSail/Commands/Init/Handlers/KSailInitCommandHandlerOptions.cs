using Devantler.KubernetesGenerator.KSail.Models;
using Devantler.KubernetesGenerator.KSail.Models.Init;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandlerOptions
{
  public required string ClusterName { get; set; }
  public KSailKubernetesDistribution Distribution { get; set; }
  public required string OutputPath { get; set; }
  public KSailInitTemplate Template { get; set; }
  public bool EnableSOPS { get; set; }
  public bool IncludeComponents { get; set; }
  public bool IncludeVariables { get; set; }
  public bool IncludeHelmReleases { get; set; }
}
