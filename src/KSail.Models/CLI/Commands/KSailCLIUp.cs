using System.ComponentModel;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'up' command.
/// </summary>
public class KSailCLIUp
{
  /// <summary>
  /// Destroy any existing cluster before creating a new one.
  /// </summary>
  [Description("Destroy any existing cluster before creating a new one.")]
  public bool Destroy { get; set; }
  /// <summary>
  /// Lint the manifests before applying them.
  /// </summary>
  [Description("Lint the manifests before applying them.")]
  public bool Lint { get; set; } = true;

  /// <summary>
  /// Wait for reconciliation to succeed.
  /// </summary>
  [Description("Wait for reconciliation to succeed.")]
  public bool Reconcile { get; set; } = true;
}
