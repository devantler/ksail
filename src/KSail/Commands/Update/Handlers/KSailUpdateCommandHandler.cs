using KSail.Provisioners;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  internal static async Task HandleAsync(string name, string manifestsPath) =>
    // Validate
    await FluxProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
}
