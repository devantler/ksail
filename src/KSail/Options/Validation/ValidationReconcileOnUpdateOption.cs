using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Validation;

/// <summary>
/// Reconcile manifests when updating a cluster.
/// </summary>
/// <param name="config"></param>
public class ValidationReconcileOnUpdateOption(KSailCluster config) : Option<bool?>(
  ["--reconcile", "-r"],
  $"Reconcile manifests. [default: {config.Spec.Validation.ReconcileOnUpdate}]"
)
{
}
