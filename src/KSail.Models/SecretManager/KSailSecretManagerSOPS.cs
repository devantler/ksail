using System.ComponentModel;

namespace KSail.Models.SecretManager;

/// <summary>
/// Options for the SOPS secret manager.
/// </summary>
public class KSailSecretManagerSOPS
{
  /// <summary>
  /// Public key used for encryption.
  /// </summary>
  [Description("Public key used for encryption.")]
  public string? PublicKey { get; set; }

  /// <summary>
  /// Use in-place decryption/encryption.
  /// </summary>
  [Description("Use in-place decryption/encryption.")]
  public bool InPlace { get; set; } = false;

  /// <summary>
  /// Show all keys in the listed keys.
  /// </summary>
  [Description("Show all keys in the listed keys.")]
  public bool ShowAllKeysInListings { get; set; } = false;

  /// <summary>
  /// Show private keys in the listed keys.
  /// </summary>
  [Description("Show private keys in the listed keys.")]
  public bool ShowPrivateKeysInListings { get; set; } = false;
  // TODO: Add missing properties to the KSailSecretManager class.
}
