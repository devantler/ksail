namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLIListOptions
{
  /// <summary>
  /// Whether to list clusters from all supported distributions.
  /// </summary>
  public bool All { get; set; }
}
