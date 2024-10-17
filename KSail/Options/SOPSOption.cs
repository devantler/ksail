using System.CommandLine;

namespace KSail.Commands.Init.Options;

class SOPSOption() : Option<bool>(
  ["--sops"],
  () => false,
  "Enable SOPS support."
)
{
}
