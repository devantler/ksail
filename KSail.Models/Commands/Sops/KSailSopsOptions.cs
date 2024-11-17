namespace KSail.Models.Commands.SOPS;

/// <summary>
/// The options to use for the 'sops' command.
/// </summary>
public class KSailSOPSOptions
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  public KSailSOPSListOptions ListOptions { get; set; } = new();
}
