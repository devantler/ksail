using System.ComponentModel;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLIListOptions
{
  /// <summary>
  /// List clusters from all supported distributions.
  /// </summary>
  [Description("List clusters from all supported distributions.")]
  public bool All { get; set; }
}
