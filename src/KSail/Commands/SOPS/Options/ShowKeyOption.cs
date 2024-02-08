using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowKeyOption() : Option<bool>(
 ["--show-key"],
  "Show the full key"
);
