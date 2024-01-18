using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Up;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.Down;

/// <summary>
/// Tests for the <see cref="KSailDownCommand"/> class.
/// </summary>
[UsesVerify]
[Collection("KSail.Tests.Integration")]
public class KSailDownCommandTests
{
  /// <summary>
  /// Tests that the <c>ksail down</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async void KSailDown_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailDown_FailsAndPrintsHelp)} test...");
    //Arrange
    var console = new TestConsole();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int exitCode = await ksailDownCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail down [name]</c> command succeeds and deletes a cluster.
  /// </summary>
  [Fact]
  public async void KSailDownName_SucceedsAndDeletesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailDownName_SucceedsAndDeletesCluster)} test...");
    //Arrange
    var console = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", console);
    int upExitCode = await ksailUpCommand.InvokeAsync($"ksail --no-gitops", console);
    int downExitCode = await ksailDownCommand.InvokeAsync("ksail --delete-pull-through-registries", console);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, downExitCode);
    Assert.False(await CheckRegistriesExistsAsync());
  }

  static async Task<bool> CheckRegistriesExistsAsync()
  {
    return await DockerTestUtils.ContainerExistsAsync("proxy-docker.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-registry.k8s.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-gcr.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-ghcr.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-quay.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-mcr.microsoft.com");
  }
}
