using KSail.Commands.Lint.Handlers;
using KSail.Provisioners;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  internal static async Task HandleAsync(string name, string manifestsPath, bool noLint, bool noReconcile)
  {
    if (!noLint)
    {
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
    }
    Console.WriteLine($"📥 Pushing manifests to {name}...");
    await FluxProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
    if (!noReconcile)
    {
      Console.WriteLine();
      Console.WriteLine($"📥 Reconciling manifests on {name}...");
      await FluxProvisioner.ReconcileAsync(name);
    }
    Console.WriteLine("");
  }
}
