namespace KSail.Provisioners.ContainerEngine;

interface IContainerEngineProvisioner
{
  Task CheckReadyAsync();
  Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null);
  Task DeleteRegistryAsync(string name);
  Task<string?> GetContainerIdAsync(string name);
}
