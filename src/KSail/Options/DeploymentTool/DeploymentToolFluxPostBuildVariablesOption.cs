using System.CommandLine;
using KSail.Models;

namespace KSail.Options.DeploymentTool;

/// <summary>
/// Enable Flux post-build variables.
/// </summary>
/// <param name="config"></param>
public class DeploymentToolFluxPostBuildVariablesOption(KSailCluster config) : Option<bool>(
  ["-fpbv", "--flux-post-build-variables"],
  $"Enable support for flux post-build variables. [default: {config.Spec.DeploymentTool.Flux.PostBuildVariables}]"
)
{
}
