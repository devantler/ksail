using KSail.Provisioners;
using KSail.Provisioners.ContainerEngine;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler(IContainerEngineProvisioner containerEngineProvisioner)
{
  readonly IContainerEngineProvisioner ContainerEngineProvisioner = containerEngineProvisioner;

  internal async Task HandleAsync(string name, bool deletePullThroughRegistries = false)
  {
    await K3dProvisioner.DeprovisionAsync(name);
    if (deletePullThroughRegistries)
    {
      await DeletePullThroughRegistriesAsync();
    }

    Console.WriteLine();
  }

  async Task DeletePullThroughRegistriesAsync()
  {
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-docker.io");
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-registry.k8s.io");
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-gcr.io");
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-ghcr.io");
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-quay.io");
    await ContainerEngineProvisioner.DeleteRegistryAsync("proxy-mcr.microsoft.com");
  }
}
