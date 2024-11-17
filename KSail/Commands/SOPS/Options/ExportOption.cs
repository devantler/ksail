using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class ExportOption() : Option<string>(
  ["--export"],
  "Export a key"
);
