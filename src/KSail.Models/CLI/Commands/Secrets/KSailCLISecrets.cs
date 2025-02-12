using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Secrets;

/// <summary>
/// The options to use for the 'secrets' command.
/// </summary>
public class KSailCLISecrets
{
  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  [Description("The options to use for the 'list' command.")]
  public KSailCLISecretsList List { get; set; } = new();

}
