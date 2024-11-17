using System.CommandLine;

namespace KSail.Commands.Sops.Options;

sealed class DecryptOption() : Option<string>(
  ["--decrypt", "-d"],
  "File to decrypt"
);
