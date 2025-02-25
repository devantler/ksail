using System.ComponentModel;

namespace KSail.Models.Validation;

/// <summary>
/// Options for the validation of the manifests.
/// </summary>
public class KSailValidation
{
  /// <summary>
  /// Lint the manifests before applying them to a new cluster.
  /// </summary>
  [Description("Lint the manifests before applying them to a new cluster.")]
  public bool LintOnUp { get; set; } = true;

  /// <summary>
  /// Wait for reconciliation to succeed on a new cluster.
  /// </summary>
  [Description("Wait for reconciliation to succeed on a new cluster.")]
  public bool ReconcileOnUp { get; set; } = true;

  /// <summary>
  /// Lint the manifests before applying them to an existing cluster.
  /// </summary>
  [Description("Lint the manifests before applying them to an existing cluster.")]
  public bool LintOnUpdate { get; set; } = true;

  /// <summary>
  /// Wait for reconciliation to succeed on an existing cluster.
  /// </summary>
  [Description("Wait for reconciliation to succeed on an existing cluster.")]
  public bool ReconcileOnUpdate { get; set; } = true;
}
