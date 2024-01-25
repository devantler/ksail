using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.SOPS;

namespace KSail.Tests.Integration.Commands.SOPS;

/// <summary>
/// Tests for the <see cref="KSailSOPSCommand"/> class.
/// </summary>
public class KSailSOPSCommandTests
{
  /// <summary>
  /// Tests that the 'ksail sops' command fails and prints the help text.
  /// </summary>
  [Fact]
  public async void KSailSOPS_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    int exitCode = await ksailSOPSCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
