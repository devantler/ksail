using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Sops;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLISopsListOptions
{
  /// <summary>
  /// Only show keys found in the SOPS config file.
  /// </summary>
  [Description("Only show keys found in the SOPS config file.")]
  public bool ShowSOPSConfigKeysOnly { get; set; } = false;

  /// <summary>
  /// Show the private key in the listed keys.
  /// </summary>
  [Description("Show the private key in the listed keys.")]
  public bool ShowPrivateKey { get; set; } = true;
}
