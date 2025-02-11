using System.CommandLine;

namespace KSail.Options;

class ProjectMirrorRegistriesOption() : Option<bool>
(
  ["-mr", "--mirror-registries"],
  "Enable mirror registries."
)
{
}
