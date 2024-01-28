using KSail.Services.Provisioners.ContainerEngine;

namespace KSail.Tests.Integration.TestUtils;

sealed class DockerTestUtils(DockerProvisioner dockerProvisioner)
{
  readonly DockerProvisioner _dockerProvisioner = dockerProvisioner;
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
