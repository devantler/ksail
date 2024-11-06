using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class GenerateKeyOption() : Option<bool?>(
  ["--generate-key", "-g"],
  "Generate a new key"
);
