using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class NoSOPSOption() : Option<bool>(
 ["--no-sops", "-ns"],
  () => false,
  "Disable SOPS"
);
