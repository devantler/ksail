using System.CommandLine;

namespace KSail.Commands.Update.Options;

class NoReconcileOption : Option<bool>
{
  internal NoReconcileOption() : base(
    ["--no-reconcile", "-nr"],
    () => false,
    "Skip reconciling manifests"
  )
  {
  }
}
