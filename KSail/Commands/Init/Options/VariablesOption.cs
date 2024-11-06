using System.CommandLine;

namespace KSail.Commands.Init.Options;

class VariablesOption() : Option<bool?>(
  ["--variables"],
  "Generate ConfigMaps and Secrets for flux post-build-variables."
)
{
}
