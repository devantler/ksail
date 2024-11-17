using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class ImportOption() : Option<string>(
  ["--import"],
  "Import a key"
);
