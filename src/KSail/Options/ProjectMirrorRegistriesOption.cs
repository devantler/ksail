using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// Enable mirror registries.
/// </summary>
public class ProjectMirrorRegistriesOption() : Option<bool?>
(
  ["-mr", "--mirror-registries"],
  "Enable mirror registries."
)
{
}
