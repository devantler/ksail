using KSail.Provisioners;

namespace KSail.Commands.Down.Handlers;

internal class KSailDownCommandHandler()
{
  internal static async Task HandleAsync(string name)
  {
    await K3dProvisioner.DeprovisionAsync(name);
    await DockerProvisioner.DeleteRegistryAsync("proxy-docker.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-registry.k8s.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-gcr.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-ghcr.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-quay.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-mcr.microsoft.com");
    Console.WriteLine();
  }
}
