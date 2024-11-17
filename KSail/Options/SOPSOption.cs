using System.CommandLine;

namespace KSail.Options;

class SopsOption() : Option<bool?>(
  ["-s", "--sops"],
  "Enable Sops support."
)
{
}
