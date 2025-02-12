using System.CommandLine;

namespace KSail.Commands.Secrets.Options;

sealed class ShowPrivateKeysOption() : Option<bool?>(
  ["--show-private-keys", "-spk"],
  "Show private keys"
);
