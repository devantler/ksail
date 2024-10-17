using Devantler.K9sCLI;

namespace KSail.Models.Commands.Debug;

/// <summary>
/// The options to use for the 'debug' command.
/// </summary>
public class KSailDebugOptions
{
  /// <summary>
  /// The editor to use for viewing files while debugging.
  /// </summary>
  public Editor Editor { get; set; } = Editor.Nano;
}
