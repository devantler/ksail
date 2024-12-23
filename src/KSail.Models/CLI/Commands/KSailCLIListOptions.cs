using System.ComponentModel;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLIListOptions
{
  /// <summary>
  /// Whether to list clusters from all supported distributions.
  /// </summary>
  [Description("Whether to list clusters from all supported distributions.")]
  public bool All { get; set; }
}
