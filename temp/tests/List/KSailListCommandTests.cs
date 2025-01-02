using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.List;

namespace KSail.Tests.Commands.List;

/// <summary>
/// Tests for the <see cref="KSailListCommand"/> class.
/// </summary>
public class KSailListCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail list --help'
  /// </summary>
  [Fact]
  public async Task KSailListHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailListCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
