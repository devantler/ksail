using System.CommandLine;

namespace KSail.Options.SecretManager;


internal class SecretManagerSOPSPublicKeyOption() : Option<string?>(
  ["--public-key", "-pk"],
  $"The public key."
)
{
}
