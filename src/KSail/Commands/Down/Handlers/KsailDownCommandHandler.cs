using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler(IContainerEngineProvisioner containerEngineProvisioner, IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IContainerEngineProvisioner _containerEngineProvisioner = containerEngineProvisioner;
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal async Task<int> HandleAsync(string clusterName, CancellationToken token, bool deletePullThroughRegistries = false)
  {
    if (await _kubernetesDistributionProvisioner.DeprovisionAsync(clusterName, token) != 0)
    {
      return 1;
    }
    if (deletePullThroughRegistries)
    {
      await DeletePullThroughRegistriesAsync();
    }

    Console.WriteLine();
    return 0;
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
