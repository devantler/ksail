using System.CommandLine;
using KSail.Models;

namespace KSail.Options.SecretManager;

/// <summary>
/// Options for the SOPS secret manager.
/// </summary>
/// <param name="config"></param>
public class SecretManagerSOPSOptions(KSailCluster config)
{
  /// <summary>
  /// Option to specify the public key.
  /// </summary>
  public readonly SecretManagerSOPSPublicKeyOption PublicKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Use in-place decryption/encryption.
  /// </summary>
  public readonly SecretManagerSOPSInPlaceOption InPlaceOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Show all keys in the listed keys.
  /// </summary>
  public readonly SecretManagerSOPSShowPrivateKeysInListingsOption ShowPrivateKeysInListingsOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Show all keys in the listed keys.
  /// </summary>
  public readonly SecretManagerSOPSShowAllKeysInListingsOption ShowAllKeysInListingsOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
