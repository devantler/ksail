using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class EncryptOption() : Option<string>(
  ["--encrypt", "-e"],
  "File to encrypt"
);
