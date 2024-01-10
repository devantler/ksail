using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class SOPSOption() : Option<bool>(
 ["--sops", "-s"],
  () => true,
  "Enable SOPS"
);
