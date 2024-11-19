using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Up;
using KSail.Commands.Update;

namespace KSail.Tests.Commands.Update;

/// <summary>
/// Tests for the <see cref="KSailUpdateCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailUpdateCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail update --help'
  /// </summary>
  [Fact]
  public async Task KSailUpdateHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailUpdateCommand();

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
  public async Task KSailUpdate_WithDefaultOptions_SucceedsAndCreatesAndUpdatesDefaultCluster()
  {
    //Cleanup
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("");
    int upExitCode = await ksailUpCommand.InvokeAsync("--reconcile false --destroy");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
  }
}
