using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;

/// <summary>
/// Set the kustomize root file.
/// </summary>
/// <param name="config"></param>
public class TemplateKustomizeRootOption(KSailCluster config) : Option<string?>
(
  ["-kr", "--kustomize-root"],
  $"The kustomize root file. [default: {config.Spec.Template.Kustomize.Root}]"
)
{
}
