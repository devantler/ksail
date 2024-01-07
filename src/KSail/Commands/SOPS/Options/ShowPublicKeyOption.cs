using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowPublicKeyOption() : Option<bool>(
 ["-spubk", "--show-public-key"],
  "show the public key"
);
