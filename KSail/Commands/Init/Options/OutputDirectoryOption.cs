using System.CommandLine;

namespace KSail.Commands.Init.Options;

sealed class OutputDirectoryOption()
 : Option<string>(
    ["-o", "--output"],
    "Location to place the generated cluster output."
  )
{
}
