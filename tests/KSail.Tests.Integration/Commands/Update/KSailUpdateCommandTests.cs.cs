using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Up;
using KSail.Commands.Update;
using KSail.Tests.Integration.TestUtils;

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
  public Task InitializeAsync() => KSailTestUtils.CleanupAsync();

  [Fact]
  public async Task KSailUpdate_FailsAndPrintsHelp()
  {
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
  /// Tests that the <c>ksail update [name]</c> command succeeds and pushes updates to OCI.
  /// </summary>
  [Fact]
  public async Task KSailUpdateName_SucceedsAndPushesUpdatesToOCI()
  {
    //Arrange
    var console = new TestConsole();
    var ksailInitCommand = new KSailInitCommand();
    var ksailUpCommand = new KSailUpCommand();
    var ksailUpdateCommand = new KSailUpdateCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail", console);
    int upExitCode = await ksailUpCommand.InvokeAsync("ksail", console);
    int updateExitCode = await ksailUpdateCommand.InvokeAsync("ksail", console);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, upExitCode);
    Assert.Equal(0, updateExitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
