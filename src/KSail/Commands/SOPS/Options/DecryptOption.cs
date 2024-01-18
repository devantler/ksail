using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

sealed class DecryptOption() : Option<string>(
["--decrypt", "-d"],
  "File to decrypt"
);
