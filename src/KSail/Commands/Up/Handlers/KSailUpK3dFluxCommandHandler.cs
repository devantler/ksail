using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dFluxCommandHandler
{
  static readonly FluxProvisioner _gitOpsProvisioner = new();
  static readonly KubernetesProvisioner _kubernetesProvisioner = new();
  static readonly DockerProvisioner _dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner _secretManagementProvisioner = new();

  internal static async Task Handle(string manifestsPath, bool sops)
  {
    Console.WriteLine();
    Console.WriteLine("🧮 Creating OCI registry...");
    await _dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);

    Console.WriteLine();
    Console.WriteLine("📥 Pushing manifests to OCI registry...");
    Console.WriteLine("📥⚠️ Not implemented yet...");

    Console.WriteLine();
    _ = _kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (sops)
    {
      Console.WriteLine();
      Console.WriteLine("🔐 Adding SOPS GPG key...");
      await _secretManagementProvisioner.CreateKeysAsync();
      await _secretManagementProvisioner.DeploySecretManagementAsync();
    }

    Console.WriteLine();
    await _gitOpsProvisioner.CheckPrerequisitesAsync();
    await _gitOpsProvisioner.InstallAsync("oci://localhost:5050/manifests", manifestsPath);
  }
}
