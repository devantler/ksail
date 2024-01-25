using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ImportOption() : Option<string>(
["--import", "-i"],
  "Import a key"
);
