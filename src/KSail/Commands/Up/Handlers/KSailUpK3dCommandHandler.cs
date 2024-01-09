using KSail.Commands.Down.Handlers;
using KSail.Provisioners.Cluster;
using KSail.Provisioners.ContainerOrchestrator;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dCommandHandler
{
  static readonly K3dProvisioner _clusterProvisioner = new();
  static readonly DockerProvisioner _dockerProvisioner = new();
  internal static async Task HandleAsync(string name, bool pullThroughRegistries, string configPath)
  {
    await _dockerProvisioner.CheckReadyAsync();

    await KSailDownK3dCommandHandler.HandleAsync(name);
    if (pullThroughRegistries)
    {
      Console.WriteLine("🧮 Creating pull-through registries...");
      await _dockerProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
      Console.WriteLine();
    }

    await _clusterProvisioner.ProvisionAsync(name, pullThroughRegistries, configPath);
  }
}
