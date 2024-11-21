namespace KSail.Models.CLI.Commands;

/// <summary>
/// The options to use for the 'up' command.
/// </summary>
public class KSailCLIUpOptions
{
  /// <summary>
  /// Whether to destroy any existing cluster before creating a new one.
  /// </summary>
  public bool Destroy { get; set; }
  /// <summary>
  /// Whether to lint the manifests before applying them.
  /// </summary>
  public bool Lint { get; set; } = true;

  /// <summary>
  /// Whether to wait for reconciliation to succeed.
  /// </summary>
  public bool Reconcile { get; set; } = true;
}
