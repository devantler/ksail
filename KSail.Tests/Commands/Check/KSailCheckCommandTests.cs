using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Check;

namespace KSail.Tests.Commands.Check;

/// <summary>
/// Tests for the <see cref="KSailCheckCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailCheckCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail check</c> command fails when given invalid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailCheck_GivenInvalidKubeconfigPath_Fails()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("--kubeconfig /path/to/invalid/kubeconfig", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail check</c> command succeeds when given a valid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailCheck_GivenValidKubeconfigPathAndInvalidContext_Fails()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("--context ksail", console);

    //Assert
    Assert.Equal(1, exitCode);
  }
}
