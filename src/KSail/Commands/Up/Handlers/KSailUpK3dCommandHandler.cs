using KSail.Commands.Down.Handlers;
using KSail.Provisioners.Cluster;
using KSail.Provisioners.ContainerOrchestrator;

namespace KSail.Commands.Up.Handlers;

static class KSailUpCommandHandler
{
  static readonly K3dProvisioner clusterProvisioner = new();
  static readonly DockerProvisioner dockerProvisioner = new();
  internal static async Task HandleAsync(string name, bool pullThroughRegistries, string configPath)
  {
    await dockerProvisioner.CheckReadyAsync();

    await KSailDownCommandHandler.HandleAsync(name);
    if (pullThroughRegistries)
    {
      Console.WriteLine("🧮 Creating pull-through registries...");
      await dockerProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
      Console.WriteLine();
    }

    await clusterProvisioner.ProvisionAsync(name, pullThroughRegistries, configPath);
  }
}
