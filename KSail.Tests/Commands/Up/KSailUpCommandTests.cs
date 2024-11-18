using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;
using KSail.Commands.Update;

namespace KSail.Tests.Commands.Up;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailUpCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail up --help'
  /// </summary>
  [Fact]
  public async Task KSailUpHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailUpCommand();

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
  public async Task KSailUp_WithDefaultOptions_SucceedsAndCreatesDefaultCluster()
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
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("");
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
  }

  /// <summary>
  /// Tests that a default kind cluster is created, and the 'ksail up' command is executed successfully.
  /// </summary>
  [Fact]
  public async Task KSailUp_WithSimpleKindCluster_SucceedsAndCreatesKindCluster()
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
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("-d kind");
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
  }

  /// <summary>
  /// Tests that an advanced kind cluster is created, and the 'ksail up' command is executed successfully.
  /// </summary>
  [Fact]
  public async Task KSailUp_WithAdvancedKindCluster_SucceedsAndCreatesAdvancedKindCluster()
  {
    //Cleanup
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
    File.Delete(".sops.yaml");

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("--name ksail-advanced-kind --distribution kind --components --sops --post-build-variables --helm-releases");
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("kind-config.yaml");
    File.Delete("ksail-config.yaml");
    File.Delete(".sops.yaml");
  }

  /// <summary>
  /// Tests that a default k3d cluster is created, and the 'ksail up' command is executed successfully.
  /// </summary>
  [Fact]
  public async Task KSailUp_WithSimpleK3dCluster_SucceedsAndCreatesK3dCluster()
  {
    //Cleanup
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    File.Delete("k3d-config.yaml");
    File.Delete("ksail-config.yaml");

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("-d k3d");
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("k3d-config.yaml");
    File.Delete("ksail-config.yaml");
  }

  /// <summary>
  /// Tests that an advanced k3d cluster is created, and the 'ksail up' command is executed successfully.
  /// </summary>
  [Fact]
  public async Task KSailUp_WithAdvancedK3dCluster_SucceedsAndCreatesAdvancedK3dCluster()
  {
    //Cleanup
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    File.Delete("k3d-config.yaml");
    File.Delete("ksail-config.yaml");
    File.Delete(".sops.yaml");

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("--name ksail-advanced-k3d --distribution k3d --components --sops --post-build-variables --helm-releases");
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);

    //Cleanup
    Directory.Delete("k8s", true);
    File.Delete("k3d-config.yaml");
    File.Delete("ksail-config.yaml");
    File.Delete(".sops.yaml");
  }
}
