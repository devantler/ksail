using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Root;

namespace KSail.Tests.Commands.Root;


public class KSailRootCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  [Fact]
  public async Task KSail_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync([]);

    //Assert
    _ = await Verify(console.Error.ToString() + console.Out);
    Assert.Equal(0, exitCode);
  }


  [Fact]
  public async Task KSailHelp_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync(["--help"], console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
