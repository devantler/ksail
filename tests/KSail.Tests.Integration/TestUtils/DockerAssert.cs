using KSail.Provisioners;

namespace KSail.Tests.Integration.TestUtils;

internal static class DockerAssert
{
  internal static async Task ContainerExistsAsync(string name)
  {
    string? container = await DockerProvisioner.GetContainerIdAsync(name);
    Assert.NotNull(container);
    Assert.NotEmpty(container);
  }
}
