using System.CommandLine;

namespace KSail.Commands.Init.Options;

class ProjectMirrorRegistriesOption() : Option<bool>
(
  ["-mr", "--mirror-registries"],
  "Enable mirror registries."
)
{
}
