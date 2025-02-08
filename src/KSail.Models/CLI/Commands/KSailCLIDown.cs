using System.ComponentModel;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'down' command.
/// </summary>
public class KSailCLIDown
{
  /// <summary>
  /// Remove registries created by ksail (will remove all cached images).
  /// </summary>
  [Description("Remove registries created by ksail (will remove all cached images).")]
  public bool Registries { get; set; }
}
