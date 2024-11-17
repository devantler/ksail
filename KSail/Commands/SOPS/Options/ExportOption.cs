using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ExportOption() : Option<string>(
  ["--export"],
  "Export a key"
);
