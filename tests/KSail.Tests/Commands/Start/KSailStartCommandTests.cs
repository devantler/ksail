using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;

namespace KSail.Tests.Commands.Start;

/// <summary>
/// Tests for the <see cref="KSailStartCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailStartCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail start --help'
  /// </summary>
  [Fact]
  public async Task KSailStartHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailStartCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that a default cluster is created, and the 'ksail up' command is executed successfully.
  /// </summary>
  [Fact]
  public async Task KSailStart_WithDefaultOptions_SucceedsAndCreatesDefaultCluster()
  {
    //Cleanup
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("");
    int upExitCode = await ksailUpCommand.InvokeAsync("--reconcile false --destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
  }
}
