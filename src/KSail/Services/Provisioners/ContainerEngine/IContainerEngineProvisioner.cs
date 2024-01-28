using KSail.Enums;

namespace KSail.Services.Provisioners.ContainerEngine;

interface IContainerEngineProvisioner
{
  Task<ContainerEngineType> GetContainerEngineTypeAsync();
  Task CheckReadyAsync();
  Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null);
  Task DeleteRegistryAsync(string name);
  Task<string?> GetContainerIdAsync(string name);
}
