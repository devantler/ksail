using System.CommandLine;

namespace KSail.Commands.Init.Options;

sealed class OutputOption()
 : Option<string>(
    ["-o", "--output"],
    () => $"./",
    "Location to place the generated cluster output."
  )
{
}
