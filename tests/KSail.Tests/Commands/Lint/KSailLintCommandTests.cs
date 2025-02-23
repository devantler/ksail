using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Lint;
using KSail.Commands.Root;

namespace KSail.Tests.Commands.Lint;

/// <summary>
/// Tests for the <see cref="KSailLintCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailLintCommandTests : IDisposable
{
  /// <summary>
  /// Tests that the 'ksail lint --help'
  /// </summary>
  [Fact]
  public async Task KSailLintHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync(["lint", "--help"], console);

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
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int initExitCode = await ksailCommand.InvokeAsync(["init", "--name", "test-cluster"], console);
    int lintExitCode = await ksailCommand.InvokeAsync(["lint"], console);

    //Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, lintExitCode);
  }

  /// <summary>
  /// Tests that the 'ksail lint' command skips when given an invalid path or a path with no YAML files.
  /// </summary>
  [Fact]
  public async Task KSailLint_GivenInvalidPathOrNoYaml_ThrowsKSailException()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);

    //Act
    int lintExitCode = await ksailCommand.InvokeAsync(["lint"], console);

    //Assert
    Assert.Equal(1, lintExitCode);
  }

  /// <summary>
  /// Tests that the 'ksail lint' command succeeds when given a valid path.
  /// </summary>
  [Fact]
  public async Task KSailLint_GivenInvalidYaml_Fails()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailRootCommand(console);
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
    await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "invalid.yaml"), invalidYaml);

    //Act
    int lintExitCode = await ksailCommand.InvokeAsync(["lint"], console);

    //Assert
    Assert.Equal(1, lintExitCode);
  }

  /// <inheritdoc/>
  public void Dispose()
  {
    if (Directory.Exists("k8s"))
    {
      Directory.Delete("k8s", true);
    }

    string[] filePaths =
    [
      "ksail-config.yaml",
      "kind-config.yaml",
      "k3d-config.yaml",
      ".sops.yaml"
    ];
    foreach (string filePath in filePaths)
    {
      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }
    }
    GC.SuppressFinalize(this);
  }
}
