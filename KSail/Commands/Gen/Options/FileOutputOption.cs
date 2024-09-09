using System.CommandLine;

namespace KSail.Commands.Gen.Options;

sealed class FileOutputOption()
 : Option<string>(
    ["-o", "--output"],
    "File to write the output to."
  )
{
}
