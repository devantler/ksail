using KSail.Provisioners;
using Xunit.Sdk;

namespace KSail.Tests.Integration.TestUtils;

internal static class DockerAssert
{
  internal static async Task ContainerExistsAsync(string name) =>
    _ = await DockerProvisioner.GetContainerIdAsync(name) ??
      throw new XunitException($"Container '{name}' does not exist");
}
