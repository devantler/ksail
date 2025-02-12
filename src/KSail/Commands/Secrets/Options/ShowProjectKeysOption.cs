using System.CommandLine;

namespace KSail.Commands.Secrets.Options;

sealed class ShowProjectKeysOption() : Option<bool?>(
  ["--show-project-keys-only", "-spko"],
  "Only show keys used in the current project"
);
