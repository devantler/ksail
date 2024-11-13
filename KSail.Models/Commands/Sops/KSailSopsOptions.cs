namespace KSail.Models.Commands.Sops;

/// <summary>
/// The options to use for the 'sops' command.
/// </summary>
public class KSailSopsOptions
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  public KSailSopsListOptions ListOptions { get; set; } = new KSailSopsListOptions();
}
