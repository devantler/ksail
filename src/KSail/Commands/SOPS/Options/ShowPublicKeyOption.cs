using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

internal sealed class ShowPublicKeyOption() : Option<bool>(
 ["--show-public-key", "-spubk"],
  "Show the public key"
);
