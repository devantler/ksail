namespace KSail.Models.Commands.Down;

/// <summary>
/// The options to use for the 'down' command.
/// </summary>
public class KSailDownOptions
{
  /// <summary>
  /// Whether to remove registries created by ksail (will remove all cached images).
  /// </summary>
  public bool Registries { get; set; }
}
