using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.Lint;

namespace KSail.Tests.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailLintCommandTests : IAsyncLifetime, IDisposable
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
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-lint-test-cluster");
    var ksailInitCommand = new KSailInitCommand();
    var ksailLintCommand = new KSailLintCommand();

    //Act
    int initExitCode = await ksailInitCommand.InvokeAsync($"--name test-cluster --output {path}");
    int lintExitCode = await ksailLintCommand.InvokeAsync($"--path {path}");

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, lintExitCode);
  }

  /// <summary>
  /// Tests that the 'ksail lint' command skips when given an invalid path or a path with no YAML files.
  /// </summary>
  [Fact]
  public async Task KSailLint_GivenInvalidPathOrNoYaml_Succeeds()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-lint-invalid-path");
    var ksailLintCommand = new KSailLintCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int lintExitCode = await ksailLintCommand.InvokeAsync($"--path {path}");

    //Assert
    Assert.Equal(0, lintExitCode);
  }

  /// <summary>
  /// Tests that the 'ksail lint' command succeeds when given a valid path.
  /// </summary>
  [Fact]
  public async Task KSailLint_GivenInvalidYaml_Fails()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-lint-invalid-yaml");
    var ksailLintCommand = new KSailLintCommand();
    string invalidYaml = """
      apiVersion: v1
      kind: Pod
      metadata:
        name: my-pod
      spec:
        containers:
        - name: my-container
          image: my-image
    """;
    _ = Directory.CreateDirectory(path);
    await File.WriteAllTextAsync(Path.Combine(path, "invalid.yaml"), invalidYaml);

    //Act
    int lintExitCode = await ksailLintCommand.InvokeAsync($"--path {path}");

    //Assert
    Assert.Equal(1, lintExitCode);
  }

  /// <inheritdoc/>
  public void Dispose()
  {
    var directories = new List<string> {
      Path.Combine(Path.GetTempPath(), "ksail-lint-test-cluster"),
      Path.Combine(Path.GetTempPath(), "ksail-lint-invalid-path"),
      Path.Combine(Path.GetTempPath(), "ksail-lint-invalid-yaml")
    };
    foreach (string directory in directories)
    {
      if (Directory.Exists(directory))
      {
        Directory.Delete(directory, true);
      }
    }
    GC.SuppressFinalize(this);
  }
}
