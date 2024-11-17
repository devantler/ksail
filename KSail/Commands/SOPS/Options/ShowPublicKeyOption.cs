using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class ShowPublicKeyOption() : Option<bool?>(
  ["--show-public-key"],
  "Show the public key"
);
