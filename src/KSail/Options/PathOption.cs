using System.CommandLine;

namespace KSail.Options;

sealed class PathOption(string description, string[]? aliases = default) : Option<string?>(
    aliases ?? ["--path", "-p"],
    description
  )
{
}
