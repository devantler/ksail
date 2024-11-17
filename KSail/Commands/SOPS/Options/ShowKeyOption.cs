using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class ShowKeyOption() : Option<bool?>(
  ["--show-key"],
  "Show the full key"
);
