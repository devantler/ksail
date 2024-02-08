using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowKeyOption() : Option<bool>(
 ["--show-key", "-sk"],
  "Show the full key"
);
