using KSail.Provisioners;

namespace KSail.Commands.Down.Handlers;

internal class KSailDownCommandHandler()
{
  internal static async Task HandleAsync(string name, bool deletePullThroughRegistries = false)
  {
    await K3dProvisioner.DeprovisionAsync(name);
    if (deletePullThroughRegistries)
    {
      await DeletePullThroughRegistriesAsync();
    }

    Console.WriteLine();
  }

  private static async Task DeletePullThroughRegistriesAsync()
  {
    await DockerProvisioner.DeleteRegistryAsync("proxy-docker.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-registry.k8s.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-gcr.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-ghcr.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-quay.io");
    await DockerProvisioner.DeleteRegistryAsync("proxy-mcr.microsoft.com");
  }
}
