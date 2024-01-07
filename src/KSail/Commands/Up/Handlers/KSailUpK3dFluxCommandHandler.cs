using KSail.Commands.Update.Handlers;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.SecretManagement;
using KSail.Utils;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dFluxCommandHandler
{
  static readonly FluxProvisioner _gitOpsProvisioner = new();
  static readonly KubernetesProvisioner _kubernetesProvisioner = new();
  static readonly DockerProvisioner _dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner _secretManagementProvisioner = new();

  internal static async Task Handle(bool shouldPrompt, string name, string manifestsPath, string fluxKustomizationPath, bool sops)
  {
    if (shouldPrompt)
    {
      manifestsPath = ConsoleUtils.Prompt("Path to k8s manifests", "./k8s", RegexFilters.PathFilter());
      fluxKustomizationPath = ConsoleUtils.Prompt("Path to Flux kustomization relative to the manifests folder", $"./clusters/{name}/flux", RegexFilters.PathFilter());
      sops = bool.Parse(ConsoleUtils.Prompt("Use SOPS", "true", RegexFilters.YesNoFilter()));
    }
    Console.WriteLine();
    await _dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);

    Console.WriteLine();
    await KSailUpdateCommandHandler.Handle(name, manifestsPath);

    Console.WriteLine();
    await _kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (sops)
    {
      Console.WriteLine();
      Console.WriteLine("🔐 Adding SOPS GPG key...");
      await _secretManagementProvisioner.CreateKeysAsync();
      await _secretManagementProvisioner.DeploySecretManagementAsync();
    }

    Console.WriteLine();
    await _gitOpsProvisioner.CheckPrerequisitesAsync();
    await _gitOpsProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", fluxKustomizationPath);
  }
}
