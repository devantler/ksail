using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Sops;

/// <summary>
/// The options to use for the 'list' command.
/// </summary>
public class KSailCLISopsListOptions
{
  /// <summary>
  /// Whether to only show keys found in the SOPS config file.
  /// </summary>
  [Description("Only show keys found in the SOPS config file.")]
  public bool ShowSOPSConfigKeysOnly { get; set; } = false;

  /// <summary>
  /// Whether to show the private key in the listed keys.
  /// </summary>
  [Description("Show the private key in the listed keys.")]
  public bool ShowPrivateKey { get; set; } = true;
}
