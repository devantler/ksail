using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Down;

namespace KSail.Tests.Integration.Commands.Down;

/// <summary>
/// Tests for the <see cref="KSailDownCommand"/> class.
/// </summary>
public class KSailDownCommandTests
{
  /// <summary>
  /// Tests that the <c>ksail down</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async Task KSailDown_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var token = default(CancellationToken);
    var ksailDownCommand = new KSailDownCommand(token);

    //Act
    int exitCode = await ksailDownCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
