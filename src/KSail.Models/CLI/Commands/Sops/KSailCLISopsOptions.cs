namespace KSail.Models.CLI.Commands.Sops;

/// <summary>
/// The options to use for the 'sops' command.
/// </summary>
public class KSailCLISopsOptions
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  public KSailCLISopsListOptions ListOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'edit' command.
  /// </summary>
  public KSailCLISopsEditOptions EditOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'encrypt' command.
  /// </summary>
  public KSailCLISopsEncryptOptions EncryptOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'decrypt' command.
  /// </summary>
  public KSailCLISopsDecryptOptions DecryptOptions { get; set; } = new();
}
