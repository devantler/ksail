using KSail.Commands.Update.Handlers;
using KSail.Provisioners;
using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.Up.Handlers;

static class KSailUpGitOpsCommandHandler
{
  static readonly KubernetesProvisioner kubernetesProvisioner = new();
  static readonly DockerProvisioner dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner secretManagementProvisioner = new();

  internal static async Task HandleAsync(string name, string manifestsPath, string fluxKustomizationPath, bool sops)
  {
    fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"./clusters/{name}" : fluxKustomizationPath;

    Console.WriteLine("🧮 Creating OCI registry...");
    await dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);
    Console.WriteLine();

    await KSailUpdateCommandHandler.HandleAsync(name, manifestsPath);
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (sops)
    {
      Console.WriteLine("🔐 Adding SOPS GPG key...");
      await SOPSProvisioner.CreateKeysAsync();
      await secretManagementProvisioner.ProvisionAsync();
      await SOPSProvisioner.CreateSOPSConfigAsync(manifestsPath);
      Console.WriteLine();
    }

    await FluxProvisioner.CheckPrerequisitesAsync();
    await FluxProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", fluxKustomizationPath);
  }
}
