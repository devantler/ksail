using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class SOPSOption() : Option<bool>(
 ["-s", "--sops"],
  () => true,
  "enable SOPS"
);
