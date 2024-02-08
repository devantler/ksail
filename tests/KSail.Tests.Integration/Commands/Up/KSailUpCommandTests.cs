using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.SOPS;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;
using KSail.Commands.Update;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.Up;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailUpCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => KSailTestUtils.CleanupAsync();

  /// <summary>
  /// Tests that the <c>ksail up</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async Task KSailUpNoNameAndNoConfig_FailsAndPrintsHelp()
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
  /// Tests that the <c>ksail up [clusterName]</c> command fails and prints help when no config exists.
  /// </summary>
  [Fact]
  public async Task KSailUpNameAndNoConfig_FailsAndPrintsHelp()
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
  /// Tests that the <c>ksail up</c> command fails and prints help when a config exists.
  /// </summary>
  [Fact]
  public async Task KSailUpNoNameAndConfig_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", console);
    int upExitCode = await ksailUpCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(1, upExitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail up [clusterName]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async Task KSailUp_SucceedsAndCreatesCluster()
  {
    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail");
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail");
    int stopExitCode = await ksailStopCommand.InvokeAsync("ksail");
    int startExitCode = await ksailStartCommand.InvokeAsync("ksail");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("ksail");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.True(await new DockerTestUtils().CheckRegistriesExistAsync());
  }

  /// <summary>
  /// Tests that the <c>ksail up [clusterName]</c> command with environment variables succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async Task KSailUpEnv_SucceedsAndCreatesCluster()
  {
    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailSOPSCommand = new KSailSOPSCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key");
    }
    string key = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"));
    Environment.SetEnvironmentVariable("KSAIL_SOPS_KEY", key);
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail");
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail");
    int stopExitCode = await ksailStopCommand.InvokeAsync("ksail");
    int startExitCode = await ksailStartCommand.InvokeAsync("ksail");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("ksail");
    Environment.SetEnvironmentVariable("KSAIL_SOPS_KEY", null);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.True(await new DockerTestUtils().CheckRegistriesExistAsync());
  }
}
