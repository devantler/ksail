using System.ComponentModel;
using KSail.Models.CLI.Commands;
using KSail.Models.CLI.Commands.Sops;

namespace KSail.Models.CLI;

/// <summary>
/// The options to use for the KSail CLI commands.
/// </summary>
public class KSailCLIOptions
{
  /// <summary>
  /// The options to use for the 'debug' command.
  /// </summary>
  [Description("The options to use for the 'debug' command.")]
  public KSailCLIDebugOptions DebugOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'down' command.
  /// </summary>
  [Description("The options to use for the 'down' command.")]
  public KSailCLIDownOptions DownOptions { get; set; } = new();
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  [Description("The options to use for the 'list' command.")]
  public KSailCLIListOptions ListOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'sops' command.
  /// </summary>
  [Description("The options to use for the 'sops' command.")]
  public KSailCLISopsOptions SopsOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'up' command.
  /// </summary>
  [Description("The options to use for the 'up' command.")]
  public KSailCLIUpOptions UpOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'update' command.
  /// </summary>
  [Description("The options to use for the 'update' command.")]
  public KSailCLIUpdateOptions UpdateOptions { get; set; } = new();
}
