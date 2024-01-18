using KSail.Commands.Down.Handlers;
using KSail.Provisioners;

namespace KSail.Commands.Up.Handlers;

static class KSailUpCommandHandler
{
  internal static async Task HandleAsync(string name, string configPath)
  {
    await DockerProvisioner.CheckReadyAsync();

    if (await K3dProvisioner.ExistsAsync(name))
    {
      await KSailDownCommandHandler.HandleAsync(name);
    }

    Console.WriteLine("🧮 Creating pull-through registries...");
    await DockerProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
    await DockerProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
    await DockerProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
    await DockerProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
    await DockerProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
    await DockerProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
    Console.WriteLine();

    await K3dProvisioner.ProvisionAsync(name, configPath);
  }
}
