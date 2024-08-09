using KSail.Provisioners.ContainerEngine;

namespace KSail.Tests.TestUtils;

sealed class DockerTestUtils()
{
  readonly DockerProvisioner _dockerProvisioner = new();
  internal async Task<bool> CheckRegistriesExistAsync()
  {
    return await ContainerExistsAsync("proxy-docker.io").ConfigureAwait(false)
      && await ContainerExistsAsync("proxy-registry.k8s.io").ConfigureAwait(false)
      && await ContainerExistsAsync("proxy-gcr.io").ConfigureAwait(false)
      && await ContainerExistsAsync("proxy-ghcr.io").ConfigureAwait(false)
      && await ContainerExistsAsync("proxy-quay.io").ConfigureAwait(false)
      && await ContainerExistsAsync("proxy-mcr.microsoft.com").ConfigureAwait(false);
  }
  internal async Task<bool> ContainerExistsAsync(string name)
  {
    var (exitCode, container) = await _dockerProvisioner.GetContainerIdAsync(name, new CancellationToken()).ConfigureAwait(false);
    return exitCode == 0 && !string.IsNullOrEmpty(container);
  }
}
