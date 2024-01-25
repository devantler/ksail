using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ExportOption() : Option<string>(
["--export", "-ex"],
  "Export a key"
);
