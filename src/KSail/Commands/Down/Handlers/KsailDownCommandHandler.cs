using KSail.Services.Provisioners.ContainerEngine;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler(IContainerEngineProvisioner containerEngineProvisioner, IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IContainerEngineProvisioner _containerEngineProvisioner = containerEngineProvisioner;
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal async Task HandleAsync(string clusterName, bool deletePullThroughRegistries = false)
  {
    await _kubernetesDistributionProvisioner.DeprovisionAsync(clusterName);
    if (deletePullThroughRegistries)
    {
      await DeletePullThroughRegistriesAsync();
    }

    Console.WriteLine();
  }

  async Task DeletePullThroughRegistriesAsync()
  {
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-docker.io");
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-registry.k8s.io");
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-gcr.io");
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-ghcr.io");
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-quay.io");
    await _containerEngineProvisioner.DeleteRegistryAsync("proxy-mcr.microsoft.com");
  }
}
