using KSail.Provisioners;

namespace KSail.Tests.Integration.TestUtils;

static class DockerTestUtils
{
  internal static async Task<bool> ContainerExistsAsync(string name)
  {
    string? container = await DockerProvisioner.GetContainerIdAsync(name);
    return !string.IsNullOrEmpty(container);
  }
}
