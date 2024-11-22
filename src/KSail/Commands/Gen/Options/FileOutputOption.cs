using System.CommandLine;

namespace KSail.Commands.Gen.Options;

sealed class FileOutputOption(string path)
 : Option<string>(
    ["-o", "--output"],
    () => path,
    "File to write the output to."
  )
{
}
