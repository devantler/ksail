using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;

namespace KSail.Tests.Commands.Init;

/// <summary>
/// Tests for the <see cref="KSailInitCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailInitCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail init' command succeeds and returns the introduction and help text.
  /// </summary>
  [Fact]
  public async Task KSailInitHelp_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailInitCommand();

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail init' command with the default options succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithDefaultOptions_SucceedsAndGeneratesKSailProject()
  {
    //Cleanup
    string outputPath = Path.Combine(Path.GetTempPath(), "ksail-init-simple");
    if (Directory.Exists(outputPath))
    {
      Directory.Delete(outputPath, true);
    }
    File.Delete("ksail-config.yaml");

    //Arrange
    var ksailCommand = new KSailInitCommand();
    if (Directory.Exists(outputPath))
    {
      Directory.Delete(outputPath, true);
    }
    _ = Directory.CreateDirectory(outputPath);

    //Act
    int exitCode = await ksailCommand.InvokeAsync($"--output {outputPath}");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(Directory.Exists(outputPath));
    foreach (string file in Directory.GetFiles(outputPath, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      string relativefilePath = file.Replace(outputPath, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("simple", directoryPath!)).UseFileName(fileName);
    }

    //Cleanup
    Directory.Delete(outputPath, true);
  }

  /// <summary>
  /// Tests that the 'ksail init' command advanced options succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithAdvancedOptions_SucceedsAndGeneratesKSailProject()
  {
    //Cleanup
    string outputPath = Path.Combine(Path.GetTempPath(), "ksail-init-advanced");
    if (Directory.Exists(outputPath))
    {
      Directory.Delete(outputPath, true);
    }
    File.Delete("ksail-config.yaml");

    //Arrange
    var ksailCommand = new KSailInitCommand();
    if (Directory.Exists(outputPath))
    {
      Directory.Delete(outputPath, true);
    }
    _ = Directory.CreateDirectory(outputPath);

    //Act
    int exitCode = await ksailCommand.InvokeAsync($"--output {outputPath} --components --post-build-variables --sops --helm-releases");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(Directory.Exists(outputPath));
    foreach (string file in Directory.GetFiles(outputPath, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      if (fileName == ".sops.yaml")
      {
        continue;
      }
      string relativefilePath = file.Replace(outputPath, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("advanced", directoryPath!)).UseFileName(fileName);
    }

    //Cleanup
    Directory.Delete(outputPath, true);
  }
}
