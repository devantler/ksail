using System.CommandLine;

namespace KSail.Commands.Init.Options;

class FluxDeploymentToolPostBuildVariablesOption() : Option<bool>(
  ["-fpbv", "--flux-post-build-variables"],
  "Generate ConfigMaps and Secrets for flux post-build-variables."
)
{
}
