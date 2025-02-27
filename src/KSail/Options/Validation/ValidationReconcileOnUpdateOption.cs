using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Validation;



internal class ValidationReconcileOnUpdateOption(KSailCluster config) : Option<bool?>(
  ["--reconcile", "-r"],
  $"Reconcile manifests. [default: {config.Spec.Validation.ReconcileOnUpdate}]"
)
{
}
