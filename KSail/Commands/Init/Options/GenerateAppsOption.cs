using System.CommandLine;

namespace KSail.Commands.Init.Options;

sealed class GenerateAppsOption()
 : Option<bool>(
    ["-a", "--apps"],
    () => true,
    "Whether or not to generate apps."
  )
{
}
