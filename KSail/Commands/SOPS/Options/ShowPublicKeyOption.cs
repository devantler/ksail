using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class ShowPublicKeyOption() : Option<bool?>(
  ["--public-key"],
  "Show the public key"
);
