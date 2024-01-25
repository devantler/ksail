using KSail.Provisioners;

namespace KSail.Tests.Integration.TestUtils;

static class DockerTestUtils
{
  internal static async Task<bool> CheckRegistriesExistAsync()
  {
    return await ContainerExistsAsync("proxy-docker.io")
      && await ContainerExistsAsync("proxy-registry.k8s.io")
      && await ContainerExistsAsync("proxy-gcr.io")
      && await ContainerExistsAsync("proxy-ghcr.io")
      && await ContainerExistsAsync("proxy-quay.io")
      && await ContainerExistsAsync("proxy-mcr.microsoft.com");
  }
  internal static async Task<bool> ContainerExistsAsync(string name)
  {
    string? container = await DockerProvisioner.GetContainerIdAsync(name);
    return !string.IsNullOrEmpty(container);
  }
}
