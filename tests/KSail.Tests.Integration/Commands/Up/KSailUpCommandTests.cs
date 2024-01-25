using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.SOPS;
using KSail.Commands.Up;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.Up;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail Tests Collection")]
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
    Console.WriteLine($"🧪 Running {nameof(KSailUpNoNameAndNoConfig_FailsAndPrintsHelp)} test...");
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
  /// Tests that the <c>ksail up [name]</c> command fails and prints help when no config exists.
  /// </summary>
  [Fact]
  public async Task KSailUpNameAndNoConfig_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndNoConfig_FailsAndPrintsHelp)} test...");
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
    Console.WriteLine($"🧪 Running {nameof(KSailUpNoNameAndConfig_FailsAndPrintsHelp)} test...");
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
  /// Tests that the <c>ksail up [name]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async Task KSailUpNameAndConfig_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndConfig_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var console = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", console);
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail", console);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.True(await DockerTestUtils.CheckRegistriesExistAsync());
  }

  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command with environment variables succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async Task KSailUpNameAndConfigAndEnv_SucceedsAndCreatesCluster()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailUpNameAndConfigAndEnv_SucceedsAndCreatesCluster)} test...");
    //Arrange
    var console = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    string key = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"));
    Environment.SetEnvironmentVariable("KSAIL_SOPS_KEY", key);
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", console);
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail", console);
    Environment.SetEnvironmentVariable("KSAIL_SOPS_KEY", null);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.True(await DockerTestUtils.CheckRegistriesExistAsync());
  }
}
