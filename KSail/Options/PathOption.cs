using System.CommandLine;

namespace KSail.Options;

sealed class PathOption(string defaultPath, string description, string[]? aliases = default)
 : Option<string>(
    aliases ?? ["--path", "-p"],
    () => defaultPath,
    description
  )
{
}
