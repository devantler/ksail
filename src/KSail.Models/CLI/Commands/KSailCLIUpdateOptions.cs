using System.ComponentModel;

namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'update' command.
/// </summary>
public class KSailCLIUpdateOptions
{
  /// <summary>
  /// Whether to lint the manifests before applying them.
  /// </summary>
  [Description("Whether to lint the manifests before applying them.")]
  public bool Lint { get; set; } = true;

  /// <summary>
  /// Whether to wait for reconciliation to succeed.
  /// </summary>
  [Description("Whether to wait for reconciliation to succeed.")]
  public bool Reconcile { get; set; } = true;
}
