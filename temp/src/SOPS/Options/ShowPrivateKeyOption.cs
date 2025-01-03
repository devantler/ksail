using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowPrivateKeyOption() : Option<bool?>(
  ["--private-key", "-p"],
  "Show the private key"
);
