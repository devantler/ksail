using System.CommandLine;

namespace KSail.Options;

sealed class OutputOption(string path, string description = "The output file to write the resource to.")
 : Option<string>(
    ["-o", "--output"],
    () => path,
    description
  )
{
}
