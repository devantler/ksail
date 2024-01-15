using System.CommandLine;
using KSail.Provisioners;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  internal static async Task HandleAsync(IConsole console, string name, string manifestsPath)
  {
    console.WriteLine($"📥 Pushing manifests to {name}...");
    await FluxProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
    console.WriteLine("");
  }
}
