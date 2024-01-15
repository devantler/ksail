using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Provisioners;

namespace KSail.Commands.Up.Handlers;

static class KSailUpGitOpsCommandHandler
{
  static readonly KubernetesProvisioner kubernetesProvisioner = new();
  static readonly DockerProvisioner dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner secretManagementProvisioner = new();

  internal static async Task HandleAsync(IConsole console, string name, string manifestsPath, string fluxKustomizationPath, bool sops)
  {
    fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"./clusters/{name}" : fluxKustomizationPath;

    console.WriteLine("🧮 Creating OCI registry...");
    await dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);
    console.WriteLine();

    await KSailUpdateCommandHandler.HandleAsync(console, name, manifestsPath);
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (sops)
    {
      console.WriteLine("🔐 Adding SOPS key...");
      await SOPSProvisioner.CreateKeysAsync();
      await secretManagementProvisioner.ProvisionAsync();
      await SOPSProvisioner.CreateSOPSConfigAsync($"{manifestsPath}/../.sops.yaml");
      console.WriteLine();
    }

    await FluxProvisioner.CheckPrerequisitesAsync();
    await FluxProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", fluxKustomizationPath);
  }
}
