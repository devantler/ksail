using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
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
  public async Task KSailLint_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var token = default(CancellationToken);
    var ksailCommand = new KSailLintCommand(token);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail lint [clusterName]' command succeeds.
  /// </summary>
  [Fact]
  public async Task KSailLint_SucceedsAndLintsCluster()
  {
    //Arrange
    var token = default(CancellationToken);
    var ksailInitCommand = new KSailInitCommand(token);
    var ksailLintCommand = new KSailLintCommand(token);

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("test");
    int lintExitCode = await ksailLintCommand.InvokeAsync("test");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, lintExitCode);
  }
}
