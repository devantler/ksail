using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// A file or directory path.
/// </summary>
public class GenericPathOption(string? path = default, string[]? aliases = default) : Option<string?>(
  aliases ?? ["-o", "--output"],
  () => path,
  "A file or directory path."
)
{ }
