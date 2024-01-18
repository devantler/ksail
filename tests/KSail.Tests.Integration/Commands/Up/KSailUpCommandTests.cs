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
[Collection("KSail.Tests.Integration")]
public class KSailUpCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail up</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async void KSailUp_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUp_FailsAndPrintsHelp)} test...");
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
  public async void KSailUpName_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpName_FailsAndPrintsHelp)} test...");
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
  public async void KSailUpNameConfigNoGitOps_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameConfigNoGitOps_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync($"ksail --config {Directory.GetCurrentDirectory()}/assets/k3d/k3d-config.yaml --no-gitops", new TestConsole());
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(await CheckRegistriesExistAsync());
    _ = await Verify(clusters);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name] --config [config-path] --manifests [manifests-path]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailUpNameConfigManifests_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameConfigManifests_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync($"ksail --config {Directory.GetCurrentDirectory()}/assets/k3d/k3d-config.yaml --manifests {Directory.GetCurrentDirectory()}/assets/k8s", new TestConsole());
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(await CheckRegistriesExistAsync());
    _ = await Verify(clusters);
  }

  static async Task<bool> CheckRegistriesExistAsync()
  {
    return await DockerTestUtils.ContainerExistsAsync("proxy-docker.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-registry.k8s.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-gcr.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-ghcr.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-quay.io")
      && await DockerTestUtils.ContainerExistsAsync("proxy-mcr.microsoft.com");
  }

  /// <inheritdoc/>
  public async Task DisposeAsync()
  {
    var ksailDownCommand = new KSailDownCommand();
    _ = await ksailDownCommand.InvokeAsync("ksail --delete-pull-through-registries");
  }
}
