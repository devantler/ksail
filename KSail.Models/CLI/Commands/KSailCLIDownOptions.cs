namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'down' command.
/// </summary>
public class KSailCLIDownOptions
{
  /// <summary>
  /// Whether to remove registries created by ksail (will remove all cached images).
  /// </summary>
  public bool Registries { get; set; }
}
