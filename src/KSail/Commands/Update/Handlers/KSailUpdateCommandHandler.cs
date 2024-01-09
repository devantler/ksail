using KSail.Provisioners.GitOps;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  static readonly FluxProvisioner gitOpsProvisioner = new();
  internal static async Task HandleAsync(string name, string manifestsPath) =>
    // Validate
    await gitOpsProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
}
