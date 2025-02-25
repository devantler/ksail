using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;

/// <summary>
/// The kustomize hooks to include in the initialized cluster.
/// </summary>
public class TemplateKustomizeHookOption(KSailCluster config) : Option<string[]?>
(
  ["-kh", "--kustomize-hook"],
  $"The kustomize hooks. [default: [{string.Join(", ", config.Spec.Template.Kustomize.Hooks)}]]"
)
{
}
