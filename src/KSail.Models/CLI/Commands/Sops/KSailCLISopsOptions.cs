using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Sops;

/// <summary>
/// The options to use for the 'sops' command.
/// </summary>
public class KSailCLISopsOptions
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  [Description("The options to use for the 'list' command.")]
  public KSailCLISopsListOptions ListOptions { get; set; } = new();

}
