using System.CommandLine;

namespace KSail.Commands.Init.Options;

class PostBuildVariablesOption() : Option<bool?>(
  ["-pbv", "--post-build-variables"],
  "Generate ConfigMaps and Secrets for flux post-build-variables."
)
{
}
