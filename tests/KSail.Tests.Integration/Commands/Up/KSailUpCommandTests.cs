using System.CommandLine;
using System.CommandLine.IO;
using KSail.CLIWrappers;
using KSail.Commands.Down;
using KSail.Commands.Up;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.Up;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[UsesVerify]
public class KSailUpCommandTests : IDisposable
{
  /// <summary>
  /// Tests that the <c>ksail up</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async void KSailUpFailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async void KSailUpNameFailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("ksail", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name] --config [config-path] --no-gitops</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailUpNameConfigNoGitOpsSucceedsAndCreatesCluster()
  {
    //Arrange
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync($"ksail --config {Directory.GetCurrentDirectory()}/assets/k3d/k3d-config.yaml --no-gitops", new TestConsole());
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, exitCode);
    await AssertRegistriesExist();
    _ = await Verify(clusters);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name] --config [config-path] --manifests [manifests-path]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailUpNameConfigManifestsSucceedsAndCreatesCluster()
  {
    //Arrange
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync($"ksail --config {Directory.GetCurrentDirectory()}/assets/k3d/k3d-config.yaml --manifests {Directory.GetCurrentDirectory()}/assets/k8s", new TestConsole());
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, exitCode);
    await AssertRegistriesExist();
    _ = await Verify(clusters);
  }

  private static async Task AssertRegistriesExist()
  {
    await DockerAssert.ContainerExistsAsync("proxy-docker.io");
    await DockerAssert.ContainerExistsAsync("proxy-registry.k8s.io");
    await DockerAssert.ContainerExistsAsync("proxy-gcr.io");
    await DockerAssert.ContainerExistsAsync("proxy-ghcr.io");
    await DockerAssert.ContainerExistsAsync("proxy-quay.io");
    await DockerAssert.ContainerExistsAsync("proxy-mcr.microsoft.com");
  }

  /// <inheritdoc/>
  public async void Dispose()
  {
    var ksailDownCommand = new KSailDownCommand();
    _ = await ksailDownCommand.InvokeAsync("ksail");

    GC.SuppressFinalize(this);
  }
}
