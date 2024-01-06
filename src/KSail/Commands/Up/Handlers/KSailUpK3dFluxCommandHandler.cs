using k8s;
using k8s.Models;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.Registry;
using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.Up.Handlers;

/// <summary>
/// The command handler responsible for handling the <c>ksail up k3d flux</c> command.
/// </summary>
public static class KSailUpK3dFluxCommandHandler
{
  static readonly FluxProvisioner _gitOpsProvisioner = new();
  static readonly DockerRegistryProvisioner _dockerRegistryProvisioner = new();
  static readonly SOPSProvisioner _secretManagementProvisioner = new();

  /// <summary>
  /// Handles the <c>ksail up k3d flux</c> command.
  /// </summary>
  /// <param name="manifestsPath">The path to your K8s manifests.</param>
  /// <param name="sops">Whether or not to use SOPS for secret management.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public static async Task Handle(string manifestsPath, bool sops)
  {
    Console.WriteLine();
    Console.WriteLine("🧮 Creating OCI registry...");
    await _dockerRegistryProvisioner.CreateRegistryAsync("manifests", 5050);

    Console.WriteLine();
    Console.WriteLine("📥 Pushing manifests to OCI registry...");
    Console.WriteLine("📥⚠️ Not implemented yet...");

    Console.WriteLine();
    Console.WriteLine("🌐 Create flux-system namespace...");
    var client = new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig());
    var fluxSystemNamespace = new V1Namespace
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta
      {
        Name = "flux-system"
      }
    };
    _ = await client.CreateNamespaceAsync(fluxSystemNamespace);
    Console.WriteLine("🌐✅ Created flux-system namespace successfully...");


    if (sops)
    {
      Console.WriteLine();
      Console.WriteLine("🔐 Adding SOPS GPG key...");
      await _secretManagementProvisioner.CreateKeysAsync();
      await _secretManagementProvisioner.DeploySecretManagementAsync();
    }

    Console.WriteLine();
    Console.WriteLine("🔄 Installing Flux GitOps...");
    Console.WriteLine("🔄⚠️ Not implemented yet...");
  }
}
