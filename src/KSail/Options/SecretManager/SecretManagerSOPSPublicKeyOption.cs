using System.CommandLine;

namespace KSail.Options.SecretManager;

/// <summary>
/// Option to specify the public key.
/// </summary>
public class SecretManagerSOPSPublicKeyOption() : Option<string?>(
  ["--public-key", "-pk"],
  $"The public key."
)
{
}
