using KSail.Provisioners.ContainerEngine;

namespace KSail.Tests.Integration.TestUtils;

sealed class DockerTestUtils()
{
  readonly DockerProvisioner _dockerProvisioner = new();
  internal async Task<bool> CheckRegistriesExistAsync()
  {
    return await ContainerExistsAsync("proxy-docker.io")
      && await ContainerExistsAsync("proxy-registry.k8s.io")
      && await ContainerExistsAsync("proxy-gcr.io")
      && await ContainerExistsAsync("proxy-ghcr.io")
      && await ContainerExistsAsync("proxy-quay.io")
      && await ContainerExistsAsync("proxy-mcr.microsoft.com");
  }
  internal async Task<bool> ContainerExistsAsync(string name)
  {
    string? container = await _dockerProvisioner.GetContainerIdAsync(name);
    return !string.IsNullOrEmpty(container);
  }
}
