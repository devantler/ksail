using System.CommandLine;

namespace KSail.Commands.Update.Options;

class ReconcileOption() : Option<bool>(
  ["--reconcile", "-r"],
  "Reconcile manifests after pushing an update"
)
{
}
