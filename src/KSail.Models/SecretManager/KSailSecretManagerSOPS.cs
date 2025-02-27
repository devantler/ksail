using System.ComponentModel;

namespace KSail.Models.SecretManager;


public class KSailSecretManagerSOPS
{

  [Description("Public key used for encryption.")]
  public string? PublicKey { get; set; }


  [Description("Use in-place decryption/encryption.")]
  public bool InPlace { get; set; } = false;


  [Description("Show all keys in the listed keys.")]
  public bool ShowAllKeysInListings { get; set; } = false;


  [Description("Show private keys in the listed keys.")]
  public bool ShowPrivateKeysInListings { get; set; } = false;
  // TODO: Add missing properties to the KSailSecretManager class.
}
