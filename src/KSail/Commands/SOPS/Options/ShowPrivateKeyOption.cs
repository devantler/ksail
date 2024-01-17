using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

internal sealed class ShowPrivateKeyOption() : Option<bool>(
 ["--show-private-key", "-sprivk"],
  "Show the private key"
);
