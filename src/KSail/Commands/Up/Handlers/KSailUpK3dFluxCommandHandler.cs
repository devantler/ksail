using KSail.Commands.Update.Handlers;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dFluxCommandHandler
{
  static readonly FluxProvisioner gitOpsProvisioner = new();
  static readonly KubernetesProvisioner kubernetesProvisioner = new();
  static readonly DockerProvisioner dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner secretManagementProvisioner = new();

  internal static async Task HandleAsync(string name, string manifestsPath, string fluxKustomizationPath, bool sops)
  {
    fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"./clusters/{name}/flux" : fluxKustomizationPath;

    Console.WriteLine("🧮 Creating OCI registry...");
    await dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);
    Console.WriteLine();

    await KSailUpdateCommandHandler.HandleAsync(name, manifestsPath);
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (sops)
    {
      Console.WriteLine("🔐 Adding SOPS GPG key...");
      await secretManagementProvisioner.CreateKeysAsync();
      await secretManagementProvisioner.ProvisionAsync();
      await SOPSProvisioner.CreateSOPSConfigAsync(manifestsPath);
      Console.WriteLine();
    }

    await gitOpsProvisioner.CheckPrerequisitesAsync();
    await gitOpsProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", fluxKustomizationPath);
  }
}
