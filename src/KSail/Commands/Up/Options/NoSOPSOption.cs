using System.CommandLine;

namespace KSail.Commands.Up.Options;

internal sealed class NoSOPSOption() : Option<bool>(
 ["--no-sops", "-ns"],
  () => false,
  "Disable SOPS"
);
