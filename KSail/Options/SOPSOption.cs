using System.CommandLine;

namespace KSail.Options;

class SOPSOption() : Option<bool>(
  ["--sops"],
  () => false,
  "Enable SOPS support."
)
{
}
