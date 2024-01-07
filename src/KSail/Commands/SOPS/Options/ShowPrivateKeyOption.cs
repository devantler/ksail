using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowPrivateKeyOption() : Option<bool>(
 ["-sprivk", "--show-private-key"],
  "show the private key"
);
