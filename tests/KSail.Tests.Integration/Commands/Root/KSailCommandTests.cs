using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Root;

namespace KSail.Tests.Integration.Commands.Root;

/// <summary>
/// Tests for the <see cref="KSailRootCommand"/> class.
/// </summary>
[UsesVerify]
public class KSailRootCommandTests
{
  /// <summary>
  /// Tests that the 'ksail' command succeeds and returns the introduction and help text.
  /// </summary>
  [Fact]
  public async void KSailSucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail --help' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async void KSailHelpSucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}