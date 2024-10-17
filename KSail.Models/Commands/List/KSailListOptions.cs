namespace KSail.Models.Commands.List;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailListOptions
{
  /// <summary>
  /// Whether to list clusters from all supported distributions.
  /// </summary>
  public bool All { get; set; }
}
