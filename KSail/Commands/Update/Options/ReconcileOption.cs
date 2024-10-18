using System.CommandLine;

namespace KSail.Commands.Update.Options;

class ReconcileOption : Option<bool>
{
  internal ReconcileOption() : base(
    ["--reconcile", "-r"],
    () => false,
    "Reconcile manifests after pushing an update"
  )
  {
  }
}
