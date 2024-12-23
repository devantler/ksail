using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Sops;

/// <summary>
/// The options to use for the 'sops' command.
/// </summary>
public class KSailCLISopsOptions
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  [Description("The options to use for the 'list' command.")]
  public KSailCLISopsListOptions ListOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'edit' command.
  /// </summary>
  [Description("The options to use for the 'edit' command.")]
  public KSailCLISopsEditOptions EditOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'encrypt' command.
  /// </summary>
  [Description("The options to use for the 'encrypt' command.")]
  public KSailCLISopsEncryptOptions EncryptOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'decrypt' command.
  /// </summary>
  [Description("The options to use for the 'decrypt' command.")]
  public KSailCLISopsDecryptOptions DecryptOptions { get; set; } = new();
}
