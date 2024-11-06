using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class EncryptOption() : Option<string>(
  ["--encrypt", "-e"],
  "File to encrypt"
);
