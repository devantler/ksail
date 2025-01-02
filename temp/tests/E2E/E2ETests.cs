using System.CommandLine;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.List;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;
using KSail.Commands.Update;

namespace KSail.Tests.E2E;

/// <summary>
/// E2E tests for the various distributions.
/// </summary>
[Collection("KSail.Tests")]
public class E2ETests : IAsyncLifetime
{
  /// <inheritdoc/>
  public async Task DisposeAsync() => await CleanupAsync();
  /// <inheritdoc/>
  public async Task InitializeAsync() => await CleanupAsync();

  /// <summary>
  /// Cleanup the test environment.
  /// </summary>
  /// <returns></returns>
  static Task CleanupAsync()
  {
    if (Directory.Exists("k8s"))
      Directory.Delete("k8s", true);
    if (File.Exists("kind-config.yaml"))
      File.Delete("kind-config.yaml");
    if (File.Exists("k3d-config.yaml"))
      File.Delete("k3d-config.yaml");
    if (File.Exists("ksail-config.yaml"))
      File.Delete("ksail-config.yaml");
    if (File.Exists(".sops.yaml"))
      File.Delete(".sops.yaml");
    return Task.CompletedTask;
  }

  /// <summary>
  /// Tests that the 'ksail up' command is executed successfully with various configurations.
  /// </summary>
  [Theory]
  [InlineData("")]
  [InlineData("-d kind")]
  [InlineData("--name ksail-advanced-kind --distribution kind --secret-manager sops --components --post-build-variables")]
  [InlineData("-d k3d")]
  [InlineData("--name ksail-advanced-k3d --distribution k3d --secret-manager sops --components --post-build-variables")]
  public async Task KSailUp_WithVariousConfigurations_Succeeds(string initArgs)
  {
    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailListCommand = new KSailListCommand();
    var ksailStopCommand = new KSailStopCommand();
    var ksailStartCommand = new KSailStartCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync(initArgs);
    int upExitCode = await ksailUpCommand.InvokeAsync("--destroy");
    int listExitCode = await ksailListCommand.InvokeAsync("");
    int stopExitCode = await ksailStopCommand.InvokeAsync("");
    int startExitCode = await ksailStartCommand.InvokeAsync("");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("");
    int downExitCode = await ksailDownCommand.InvokeAsync("--registries");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, listExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.Equal(0, updateExitCode);
    Assert.Equal(0, downExitCode);
  }
}
