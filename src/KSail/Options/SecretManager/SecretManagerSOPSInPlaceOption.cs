using System.CommandLine;
using KSail.Models;

namespace KSail.Options.SecretManager;

/// <summary>
/// Use in-place decryption/encryption.
/// </summary>
/// <param name="config"></param>
public class SecretManagerSOPSInPlaceOption(KSailCluster config) : Option<bool>(
  ["--in-place", "-ip"],
  $"In-place decryption/encryption. [default: {config.Spec.SecretManager.SOPS.InPlace}]"
)
{
}
