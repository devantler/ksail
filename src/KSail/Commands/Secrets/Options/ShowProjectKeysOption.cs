using System.CommandLine;

namespace KSail.Commands.Secrets.Options;

sealed class ShowProjectKeysOption() : Option<bool?>(
  ["--show-project-keys", "-spk"],
  "Only show keys used in the current project"
);
