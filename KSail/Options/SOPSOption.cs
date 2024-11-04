using System.CommandLine;

namespace KSail.Options;

class SOPSOption() : Option<bool>(
  ["-s", "--sops"],
  () => false,
  "Enable SOPS support."
)
{
}
