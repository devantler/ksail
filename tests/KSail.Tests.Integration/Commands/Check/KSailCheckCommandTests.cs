using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Check;

namespace KSail.Tests.Integration.Commands.Check;

/// <summary>
/// Tests for the <see cref="KSailCheckCommand"/> class.
/// </summary>
public class KSailCheckCommandTests
{
  /// <summary>
  /// Tests that the <c>ksail check</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async Task KSailCheck_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
