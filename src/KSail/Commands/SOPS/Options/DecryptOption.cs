using System.CommandLine;

namespace KSail.Commands.SOPS.Options;

internal sealed class DecryptOption() : Option<string>(
["--decrypt", "-d"],
  "File to decrypt"
);
