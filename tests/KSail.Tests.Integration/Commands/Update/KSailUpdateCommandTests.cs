using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.Update;
using KSail.Provisioners.ContainerEngine;

namespace KSail.Tests.Integration.Commands.Update;

/// <summary>
/// Tests for the <see cref="KSailUpdateCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailUpdateCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail update</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async Task KSailUpdate_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailUpdate_FailsAndPrintsHelp)}");
    //Arrange
    var console = new TestConsole();
    var ksailUpdateCommand = new KSailUpdateCommand();

    //Act
    int exitCode = await ksailUpdateCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
    Console.WriteLine("");
  }

  /// <summary>
  /// Tests that the <c>ksail update [clusterName] --no-reconcile</c> command succeeds and pushes updates to OCI.
  /// </summary>
  [Fact]
  public async Task KSailUpdateNameNoReconcile_SucceedsAndPushesUpdatesToOCI()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailUpdateNameNoReconcile_SucceedsAndPushesUpdatesToOCI)}");
    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();
    var dockerProvisioner = new DockerProvisioner();

    //Act
    int dockerProvisionerExitCode = await dockerProvisioner.CreateRegistryAsync("manifests", 5050, new CancellationToken());
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail");
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("ksail --no-reconcile");

    //Assert
    Assert.Equal(0, dockerProvisionerExitCode);
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, updateExitCode);
    Console.WriteLine("");
  }
}
