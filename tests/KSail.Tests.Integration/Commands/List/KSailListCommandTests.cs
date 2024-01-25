using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.List;
using KSail.Commands.Up;

namespace KSail.Tests.Integration.Commands.List;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
public class KSailListCommandTests
{
  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async Task KSailList_SucceedsAndListsClusters()
  {
    //Arrange
    var console = new TestConsole();
    var ksailListCommand = new KSailListCommand();

    //Act
    int exitCode = await ksailListCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(0, exitCode);
  }
}
