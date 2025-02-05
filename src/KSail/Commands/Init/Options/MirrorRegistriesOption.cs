using System.CommandLine;

namespace KSail.Commands.Init.Options;

class MirrorRegistriesOption() : Option<bool>
(
  ["-mr", "--mirror-registries"],
  "Enable mirror registries."
)
{
}
