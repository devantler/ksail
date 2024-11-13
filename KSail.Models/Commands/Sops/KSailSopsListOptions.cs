namespace KSail.Models.Commands.Sops;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailSopsListOptions
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
