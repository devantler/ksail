using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Debug;

namespace KSail.Tests.Commands.Debug;

/// <summary>
/// Tests for the <see cref="KSailDebugCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailDebugCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail debug --help'
  /// </summary>
  [Fact]
  public async Task KSailDebugHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailDebugCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
