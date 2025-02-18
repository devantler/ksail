using System.ComponentModel;
using KSail.Models.CLI.Commands;
using KSail.Models.CLI.Commands.Secrets;

namespace KSail.Models.CLI;

/// <summary>
/// The options to use for the KSail CLI commands.
/// </summary>
public class KSailCLI
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  [Description("The options to use for the 'list' command.")]
  public KSailCLIList List { get; set; } = new();

  /// <summary>
  /// The options to use for the 'secrets' command.
  /// </summary>
  [Description("The options to use for the 'secrets' command.")]
  public KSailCLISecrets Secrets { get; set; } = new();

  /// <summary>
  /// The options to use for the 'up' command.
  /// </summary>
  [Description("The options to use for the 'up' command.")]
  public KSailCLIUp Up { get; set; } = new();

  /// <summary>
  /// The options to use for the 'update' command.
  /// </summary>
  [Description("The options to use for the 'update' command.")]
  public KSailCLIUpdate Update { get; set; } = new();
}
