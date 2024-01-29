using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Lint;

namespace KSail.Tests.Integration.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
public class KSailLintCommandTests
{
  /// <summary>
  /// Tests that the 'ksail lint' command fails and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSail_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailLintCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
