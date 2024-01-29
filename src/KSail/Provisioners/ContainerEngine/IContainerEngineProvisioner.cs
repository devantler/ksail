namespace KSail.Provisioners.ContainerEngine;

interface IContainerEngineProvisioner
{
  Task<int> CheckReadyAsync(CancellationToken token);
  Task<int> CreateRegistryAsync(string name, int port, CancellationToken token, Uri? proxyUrl = null);
  Task<int> DeleteRegistryAsync(string name, CancellationToken token);
  Task<(int ExitCode, string? Result)> GetContainerIdAsync(string name, CancellationToken token);
}
