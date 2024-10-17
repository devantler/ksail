using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Commands.Init.Generators;

class TemplateGeneratorOptions
{
  public required string ClusterName { get; set; }
  public required KSailKubernetesDistribution Distribution { get; set; }
  public required string[] KustomizeFlows { get; set; }
  public string[] KustomizeHooks { get; set; } = [];
  public bool EnableSOPS { get; set; }
  public bool IncludeComponents { get; set; }
  public bool IncludeVariables { get; set; }
  public bool IncludeHelmReleases { get; set; } = true;
  public required string OutputPath { get; set; }
}
