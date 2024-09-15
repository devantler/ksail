using System.CommandLine;
using System.CommandLine.IO;
using System.Text.RegularExpressions;
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
  /// Tests that the 'ksail init * --output * --template k3d-flux-default' command generates a new cluster configuration in the specified output directory.
  /// </summary>
  [Fact]
  public async Task KSailInitTemplateK3dFluxDefault_SucceedsAndGeneratesClusterConfiguration()
  {
    //Arrange
    var ksailCommand = new KSailInitCommand();
    string outputPath = Path.Combine(Path.GetTempPath(), "ksail-init-k3d-flux-default");
    if (Directory.Exists(outputPath))
    {
      Directory.Delete(outputPath, true);
    }
    _ = Directory.CreateDirectory(outputPath);

    //Act
    int exitCode = await ksailCommand.InvokeAsync($"test-cluster --output {outputPath} --template K3dFluxDefault");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(Directory.Exists(outputPath));
    Dictionary<string, string> files = [];
    foreach (string file in Directory.GetFiles(outputPath, "*", SearchOption.AllDirectories))
    {
      files[file] = await File.ReadAllTextAsync(file);
    }
    // Remove age keys in age: |- from .sops.yaml file
    files[Path.Combine(outputPath, ".sops.yaml")] = Regex.Replace(files[Path.Combine(outputPath, ".sops.yaml")], @"  age: \|-\n.+", "  age: ''");
    _ = await Verify(files);

    //Cleanup
    Directory.Delete(outputPath, true);
  }
}
