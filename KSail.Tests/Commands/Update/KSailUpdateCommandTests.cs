using System.CommandLine;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KSail.Commands.Init;
using KSail.Commands.Update;
using KSail.Provisioners.ContainerEngine;

namespace KSail.Tests.Commands.Update;

/// <summary>
/// Tests for the <see cref="KSailUpdateCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailUpdateCommandTests : IAsyncLifetime
{
  readonly ResourceManager ResourceManager = new("KSail.Tests.Commands.Update.Resources", Assembly.GetExecutingAssembly());
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
    Console.WriteLine(ResourceManager.GetString(nameof(KSailUpdate_FailsAndPrintsHelp), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailUpdateCommand = new KSailUpdateCommand();

    //Act
    int exitCode = await ksailUpdateCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail update [clusterName] --no-reconcile</c> command succeeds and pushes updates to OCI.
  /// </summary>
  [Fact]
  public async Task KSailUpdateNameNoReconcile_SucceedsAndPushesUpdatesToOCI()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailUpdateNameNoReconcile_SucceedsAndPushesUpdatesToOCI), CultureInfo.InvariantCulture));
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
  }
}
