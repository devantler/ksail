using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;

/// <summary>
/// The flows to include.
/// </summary>
/// <param name="config"></param>
public class TemplateKustomizeFlowOption(KSailCluster config) : Option<string[]?>
(
  ["-kf", "--kustomize-flow"],
  $"The flows to include. [default: [{string.Join(", ", config.Spec.Template.Kustomize.Flows)}]]"
)
{
}
