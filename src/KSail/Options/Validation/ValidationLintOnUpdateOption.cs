using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Validation;

/// <summary>
/// Lint manifests before updating a cluster.
/// </summary>
/// <param name="config"></param>
public class ValidationLintOnUpdateOption(KSailCluster config) : Option<bool?>(
  ["--lint", "-l"],
  $"Lit manifests. [default: {config.Spec.Validation.LintOnUpdate}]"
)
{
}
