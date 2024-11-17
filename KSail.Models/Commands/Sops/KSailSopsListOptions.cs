namespace KSail.Models.Commands.SOPS;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailSOPSListOptions
{
  /// <summary>
  /// Whether to show the public key in the listed keys.
  /// </summary>
  public bool ShowPublicKey { get; set; } = true;

  /// <summary>
  /// Whether to show the private key in the listed keys.
  /// </summary>
  public bool ShowPrivateKey { get; set; } = true;
}
