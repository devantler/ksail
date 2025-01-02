using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowSOPSConfigKeysOnlyOption() : Option<bool?>(
  ["--sops-config-keys-only", "-s"],
  "Only show keys found in your SOPS config file"
);
