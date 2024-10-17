namespace KSail.Models.Commands.Update;

/// <summary>
/// The options to use for the 'update' command.
/// </summary>
public class KSailUpdateOptions
{
  /// <summary>
  /// Whether to lint the manifests before applying them.
  /// </summary>
  public bool Lint { get; set; }

  /// <summary>
  /// Whether to wait for reconciliation to succeed.
  /// </summary>
  public bool Reconcile { get; set; }
}
