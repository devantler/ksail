using KSail.Commands.Check.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Provisioners;

namespace KSail.Commands.Up.Handlers;

internal static class KSailUpGitOpsCommandHandler
{
  private static readonly KubernetesProvisioner kubernetesProvisioner = new();
  private static readonly DockerProvisioner dockerProvisioner = new();
  private static readonly SOPSProvisioner sopsProvisioner = new();

  internal static async Task HandleAsync(string name, string configPath, string manifestsPath, string kustomizationsPath, int timeout, bool noSOPS)
  {
    kustomizationsPath = string.IsNullOrEmpty(kustomizationsPath) ? $"clusters/{name}" : kustomizationsPath;

    Console.WriteLine("🧮 Creating OCI registry...");
    await dockerProvisioner.CreateRegistryAsync("manifests", 5050);
    Console.WriteLine("");

    await KSailUpdateCommandHandler.HandleAsync(name, manifestsPath);
    await K3dProvisioner.ProvisionAsync(name, configPath);
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (!noSOPS)
    {
      Console.WriteLine("🔐 Adding SOPS key...");
      await SOPSProvisioner.CreateKeysAsync();
      await sopsProvisioner.ProvisionAsync();
      await SOPSProvisioner.CreateSOPSConfigAsync($"{manifestsPath}/../.sops.yaml");
      Console.WriteLine("");
    }

    await FluxProvisioner.CheckPrerequisitesAsync();
    await FluxProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", kustomizationsPath);
    await KSailCheckCommandHandler.HandleAsync(name, timeout, new CancellationToken());
  }
}
