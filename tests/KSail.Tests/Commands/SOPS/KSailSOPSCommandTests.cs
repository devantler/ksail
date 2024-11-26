using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.SOPS;

namespace KSail.Tests.Commands.SOPS;

/// <summary>
/// Tests for the <see cref="KSailSOPSCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailSOPSCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail sops --help'
  /// </summary>
  [Fact]
  public async Task KSailSOPSHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailSOPSCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail sops list' command succeeds.
  /// </summary>
  [Fact]
  public async Task KSailSOPSList_WithNoLocalAgeKeys_Succeeds()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailSOPSCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("list", console);

    //Assert
    Assert.Equal(0, exitCode);
  }
}
