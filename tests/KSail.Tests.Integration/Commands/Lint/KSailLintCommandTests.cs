using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.Lint;

namespace KSail.Tests.Integration.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailLintCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail lint' command fails and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailLint_SucceedsAndPrintsIntroductionAndHelp()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailLint_SucceedsAndPrintsIntroductionAndHelp)}");
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailLintCommand();

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
    Console.WriteLine($"🧪 Running test: {nameof(KSailLint_SucceedsAndLintsCluster)}");
    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailLintCommand = new KSailLintCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail");
    int lintExitCode = await ksailLintCommand.InvokeAsync("ksail");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, lintExitCode);
  }
}
