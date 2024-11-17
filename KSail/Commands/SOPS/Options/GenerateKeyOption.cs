using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class GenerateKeyOption() : Option<bool?>(
  ["--generate-key", "-g"],
  "Generate a new key"
);
