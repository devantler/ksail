using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Validation;

/// <summary>
/// Validation options.
/// </summary>
/// <param name="config"></param>
public class ValidationOptions(KSailCluster config)
{

  /// <summary>
  /// Lint manifests before creating a cluster.
  /// </summary>
  public ValidationLintOnUpOption LintOnUpOption { get; } = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Reconile manifests when creating a cluster.
  /// </summary>
  public ValidationReconcileOnUpOption ReconcileOnUpOption { get; } = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Lint manifests before updating a cluster.
  /// </summary>
  public ValidationLintOnUpdateOption LintOnUpdateOption { get; } = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Reconcile manifests when updating a cluster.
  /// </summary>
  public ValidationReconcileOnUpdateOption ReconcileOnUpdateOption { get; } = new(config) { Arity = ArgumentArity.ZeroOrOne };

}
