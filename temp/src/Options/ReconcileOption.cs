using System.CommandLine;

namespace KSail.Options;

class ReconcileOption() : Option<bool?>(
  ["--reconcile", "-r"],
  "Reconcile manifests after pushing an update"
)
{
}
