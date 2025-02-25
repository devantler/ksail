using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Validation;

/// <summary>
/// Lint manifests before creating a cluster.
/// </summary>
/// <param name="config"></param>
public class ValidationLintOnUpOption(KSailCluster config) : Option<bool?>(
  ["--lint", "-l"],
  $"Lit manifests. [default: {config.Spec.Validation.LintOnUp}'"
)
{
}
