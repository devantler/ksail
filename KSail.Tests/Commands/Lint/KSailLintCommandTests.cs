using System.CommandLine;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KSail.Commands.Init;
using KSail.Commands.Lint;

namespace KSail.Tests.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailLintCommandTests : IAsyncLifetime
{
  readonly ResourceManager ResourceManager = new("KSail.Tests.Commands.Lint.Resources", Assembly.GetExecutingAssembly());
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
    Console.WriteLine(ResourceManager.GetString(nameof(KSailLint_SucceedsAndPrintsIntroductionAndHelp), CultureInfo.InvariantCulture));
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
    Console.WriteLine(ResourceManager.GetString(nameof(KSailLint_SucceedsAndLintsCluster), CultureInfo.InvariantCulture));
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
