using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler(IContainerEngineProvisioner containerEngineProvisioner, IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IContainerEngineProvisioner _containerEngineProvisioner = containerEngineProvisioner;
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal async Task<int> HandleAsync(string clusterName, CancellationToken token, bool deletePullThroughRegistries = false)
  {
    Console.WriteLine($"🔥 Destroying cluster '{clusterName}'");
    if (await _kubernetesDistributionProvisioner.DeprovisionAsync(clusterName, token) != 0)
    {
      return 1;
    }
    if (deletePullThroughRegistries && await DeletePullThroughRegistriesAsync(token) != 0)
    {
      return 1;
    }

    Console.WriteLine();
    return 0;
  }

  async Task<int> DeletePullThroughRegistriesAsync(CancellationToken token)
  {
    return await _containerEngineProvisioner.DeleteRegistryAsync("proxy-docker.io", token) != 0 ||
      await _containerEngineProvisioner.DeleteRegistryAsync("proxy-registry.k8s.io", token) != 0 ||
      await _containerEngineProvisioner.DeleteRegistryAsync("proxy-gcr.io", token) != 0 ||
      await _containerEngineProvisioner.DeleteRegistryAsync("proxy-ghcr.io", token) != 0 ||
      await _containerEngineProvisioner.DeleteRegistryAsync("proxy-quay.io", token) != 0 ||
      await _containerEngineProvisioner.DeleteRegistryAsync("proxy-mcr.microsoft.com", token) != 0
      ? 1
      : 0;
  }
}
