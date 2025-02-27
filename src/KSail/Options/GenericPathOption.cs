using System.CommandLine;

namespace KSail.Options;


internal class GenericPathOption(string? path = default, string[]? aliases = default) : Option<string?>(
  aliases ?? ["-o", "--output"],
  () => path,
  "A file or directory path."
)
{ }
