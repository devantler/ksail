using System.CommandLine;

namespace KSail.Commands.Init.Options;

sealed class GenerateInfrastructureOption()
  : Option<bool>(
    ["-i", "--infrastructure"],
    () => true,
    "Whether or not to generate infrastructure."
  )
{
}
