using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class ShowPrivateKeyOption() : Option<bool?>(
  ["--show-private-key"],
  "Show the private key"
);
