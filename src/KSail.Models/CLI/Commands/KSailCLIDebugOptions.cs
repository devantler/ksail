using System.ComponentModel;
using Devantler.K9sCLI;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'debug' command.
/// </summary>
public class KSailCLIDebugOptions
{
  /// <summary>
  /// The editor to use for viewing files while debugging.
  /// </summary>
  [Description("The editor to use for viewing files while debugging.")]
  public Editor Editor { get; set; } = Editor.Nano;
}
