using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Secrets;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLISecretsList
{
  /// <summary>
  /// Only show keys found in the SOPS config file.
  /// </summary>
  [Description("Only show keys used in the current project.")]
  public bool ShowProjectKeys { get; set; } = false;

  /// <summary>
  /// Show private keys in the listed keys.
  /// </summary>
  [Description("Show private keys in the listed keys.")]
  public bool ShowPrivateKeys { get; set; } = false;
}
