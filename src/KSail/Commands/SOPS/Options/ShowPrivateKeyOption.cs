using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowPrivateKeyOption() : Option<bool>(
 ["--show-private-key"],
  "Show the private key"
);
