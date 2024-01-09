using KSail.Provisioners.GitOps;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  static readonly FluxProvisioner _gitOpsProvisioner = new();
  internal static async Task HandleAsync(string name, string manifestsPath) =>
    // Validate
    await _gitOpsProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
}
