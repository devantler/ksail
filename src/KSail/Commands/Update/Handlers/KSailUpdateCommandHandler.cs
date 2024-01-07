using KSail.Provisioners.GitOps;
using KSail.Utils;

namespace KSail.Commands.Update.Handlers;

static class KSailUpdateCommandHandler
{
  static readonly FluxProvisioner _gitOpsProvisioner = new();
  internal static async Task Handle(string name, string manifestsPath)
  {
    bool shouldPrompt = string.IsNullOrEmpty(name);
    if (shouldPrompt)
    {
      name = ConsoleUtils.Prompt("Name of the cluster");
    }

    // Validate
    await _gitOpsProvisioner.PushManifestsAsync($"oci://localhost:5050/{name}", manifestsPath);
  }
}
