using System.CommandLine;
using System.CommandLine.IO;
using KSail.CLIWrappers;
using KSail.Commands.Down;
using KSail.Commands.Init;
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
  public async void KSailUpNoNameAndNoConfig_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNoNameAndNoConfig_FailsAndPrintsHelp)} test...");
    //Arrange
    var testConsole = new TestConsole();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("", testConsole);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(testConsole.Error.ToString() + testConsole.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command fails and prints help when no config exists.
  /// </summary>
  [Fact]
  public async void KSailUpNameAndNoConfig_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndNoConfig_FailsAndPrintsHelp)} test...");
    //Arrange
    var testConsole = new TestConsole();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("ksail", testConsole);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(testConsole.Error.ToString() + testConsole.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up</c> command fails and prints help when a config exists.
  /// </summary>
  [Fact]
  public async void KSailUpNoNameAndConfig_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNoNameAndConfig_FailsAndPrintsHelp)} test...");
    //Arrange
    var testConsole = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", testConsole);
    int upExitCode = await ksailUpCommand.InvokeAsync("", testConsole);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(1, upExitCode);
    _ = await Verify(testConsole.Error.ToString() + testConsole.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailUpNameAndConfig_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndConfig_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var testConsole = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", testConsole);
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail", testConsole);
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.True(await CheckRegistriesExistAsync());
    _ = await Verify(clusters);
  }

  /// <summary>
  /// Tests that the <c>ksail up [name] --no-gitops</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailUpNameAndConfigAndNoGitOps_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndConfigAndNoGitOps_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var testConsole = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", testConsole);
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail --no-gitops", testConsole);
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
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
    if (Directory.Exists("k8s"))
    {
      Directory.Delete("k8s", true);
    }
    if (File.Exists("ksail-k3d-config.yaml"))
    {
      File.Delete("ksail-k3d-config.yaml");
    }
  }
}
