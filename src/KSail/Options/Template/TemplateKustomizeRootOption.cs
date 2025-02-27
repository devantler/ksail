using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;



internal class TemplateKustomizeRootOption(KSailCluster config) : Option<string?>
(
  ["-kr", "--kustomize-root"],
  $"The kustomize root file. [default: {config.Spec.Template.Kustomize.Root}]"
)
{
}
