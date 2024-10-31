using System.CommandLine;

namespace KSail.Options;

sealed class PathOption(string defaultPath, string description)
 : Option<string>(
    ["-p", "--path"],
    () => defaultPath,
    description
  )
{
}
