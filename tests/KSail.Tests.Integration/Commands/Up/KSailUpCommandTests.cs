using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Up;

namespace KSail.Tests.Integration.Commands.Up;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[UsesVerify]
public class KSailUpCommandTests
{
  /// <summary>
  /// Tests that the 'ksail up' command fails and prints help.
  /// </summary>
  [Fact]
  public async void NoOptions_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailUpCommand = new KSailUpCommand(console);

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  [Fact]
  public async void NameOption_SucceedsAndCreatesCluster()
  {
    //Arrange
    var console = new TestConsole();
    var ksailUpCommand = new KSailUpCommand(console);

    //Act
    int exitCode = await ksailUpCommand.InvokeAsync("--name ksail-test", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
