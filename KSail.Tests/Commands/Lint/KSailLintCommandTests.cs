using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.Lint;

namespace KSail.Tests.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailLintCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail lint --help'
  /// </summary>
  [Fact]
  public async Task KSailLintHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailLintCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail lint' command succeeds when given a valid path.
  /// </summary>
  [Fact]
  public async Task KSailLint_GivenValidPath_Succeeds()
  {
    //Cleanup
    string outputPath = Path.Combine(Path.GetTempPath(), "ksail-lint-test-cluster");
    if (Directory.Exists(Path.Combine(outputPath, "k8s")))
      Directory.Delete(Path.Combine(outputPath, "k8s"), true);
    File.Delete(Path.Combine(outputPath, "kind-config.yaml"));
    File.Delete(Path.Combine(outputPath, "ksail-config.yaml"));

    //Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailLintCommand = new KSailLintCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync($"--name test-cluster --output {outputPath} --template simple");
    int lintExitCode = await ksailLintCommand.InvokeAsync($"--path {outputPath}/k8s");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, lintExitCode);

    //Cleanup
    Directory.Delete(Path.Combine(outputPath, "k8s"), true);
    File.Delete(Path.Combine(outputPath, "kind-config.yaml"));
    File.Delete(Path.Combine(outputPath, "ksail-config.yaml"));
  }
}
