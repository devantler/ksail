using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;

namespace KSail.Tests.Commands.Init;

/// <summary>
/// Tests for the <see cref="KSailInitCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailInitCommandTests : IAsyncLifetime, IDisposable
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
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-native-simple");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCode = await ksailCommand.InvokeAsync($"--path {path}");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("native-simple", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <summary>
  /// Tests that the 'ksail init' command with the default options on top of an existing project succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithDefaultOptionsOnTopOfExistingProject_SucceedsAndGeneratesKSailProject()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-native-simple-existing");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCodeRun1 = await ksailCommand.InvokeAsync($"--path {path}");
    int exitCodeRun2 = await ksailCommand.InvokeAsync($"--path {path}");

    //Assert
    Assert.Equal(0, exitCodeRun1);
    Assert.Equal(0, exitCodeRun2);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("native-simple-existing", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <summary>
  /// Tests that the 'ksail init' command with the default options and multiple clusters succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithDefaultOptionsMultipleClusters_SucceedsAndGeneratesKSailProject()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-mixed-simple-multi");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCodeRun1 = await ksailCommand.InvokeAsync($"--name cluster1 --distribution native --path {path}");
    int exitCodeRun2 = await ksailCommand.InvokeAsync($"--name cluster2 --distribution k3s --path {path}");

    //Assert
    Assert.Equal(0, exitCodeRun1);
    Assert.Equal(0, exitCodeRun2);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("mixed-simple-multi", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <summary>
  /// Tests that the 'ksail init' command advanced options succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithAdvancedOptions_SucceedsAndGeneratesKSailProject()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-native-advanced");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCode = await ksailCommand.InvokeAsync($"--name ksail-advanced-native --path {path} --secret-manager sops --components --post-build-variables --kustomization-hooks clusters/ksail-advanced-native distributions/native shared");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      if (fileName == ".sops.yaml")
      {
        continue;
      }
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("native-advanced", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <summary>
  /// Tests that the 'ksail init' command advanced options on top of an existing project succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithAdvancedOptionsOnTopOfExistingProject_SucceedsAndGeneratesKSailProject()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-native-advanced-existing");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCodeRun1 = await ksailCommand.InvokeAsync($"--name ksail-advanced-native --path {path} --secret-manager sops --components --post-build-variables --kustomization-hooks clusters/ksail-advanced-native distributions/native shared");
    int exitCodeRun2 = await ksailCommand.InvokeAsync($"--name ksail-advanced-native --path {path} --secret-manager sops --components --post-build-variables --kustomization-hooks clusters/ksail-advanced-native distributions/native shared");

    //Assert
    Assert.Equal(0, exitCodeRun1);
    Assert.Equal(0, exitCodeRun2);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      if (fileName == ".sops.yaml")
      {
        continue;
      }
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("native-advanced-existing", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <summary>
  /// Tests that the 'ksail init' command advanced options and multiple clusters succeeds and generates a KSail project.
  /// </summary>
  [Fact]
  public async Task KSailInit_WithAdvancedOptionsMultipleClusters_SucceedsAndGeneratesKSailProject()
  {
    //Arrange
    string path = Path.Combine(Path.GetTempPath(), "ksail-init-mixed-advanced-multi");
    var ksailCommand = new KSailInitCommand();
    _ = Directory.CreateDirectory(path);

    //Act
    int exitCodeRun1 = await ksailCommand.InvokeAsync($"--name cluster1 --path {path} --secret-manager sops --components --post-build-variables --distribution native --kustomization-hooks clusters/cluster1 distributions/native shared");
    int exitCodeRun2 = await ksailCommand.InvokeAsync($"--name cluster2 --path {path} --secret-manager sops --components --post-build-variables --distribution k3s --kustomization-hooks clusters/cluster2 distributions/k3s shared");

    //Assert
    Assert.Equal(0, exitCodeRun1);
    Assert.Equal(0, exitCodeRun2);
    Assert.True(Directory.Exists(path));
    foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
    {
      string fileName = Path.GetFileName(file);
      if (fileName == ".sops.yaml")
      {
        continue;
      }
      string relativefilePath = file.Replace(path, "", StringComparison.OrdinalIgnoreCase).TrimStart(Path.DirectorySeparatorChar);
      relativefilePath = relativefilePath.Replace(Path.DirectorySeparatorChar, '/');
      string? directoryPath = Path.GetDirectoryName(relativefilePath);
      _ = await Verify(await File.ReadAllTextAsync(file), extension: "yaml").UseDirectory(Path.Combine("mixed-advanced-multi", directoryPath!)).UseFileName(fileName);
    }
  }

  /// <inheritdoc/>
  public void Dispose()
  {
    string[] directoryPaths =
    [
      Path.Combine(Path.GetTempPath(), "ksail-init-mixed-advanced-multi"),
      Path.Combine(Path.GetTempPath(), "ksail-init-mixed-simple-multi"),
      Path.Combine(Path.GetTempPath(), "ksail-init-native-advanced-existing"),
      Path.Combine(Path.GetTempPath(), "ksail-init-native-advanced"),
      Path.Combine(Path.GetTempPath(), "ksail-init-native-simple-existing"),
      Path.Combine(Path.GetTempPath(), "ksail-init-native-simple")
    ];
    foreach (string path in directoryPaths)
    {
      if (Directory.Exists(path))
      {
        Directory.Delete(path, true);
      }
    }
    string[] filePaths =
    [
      "ksail-config.yaml"
    ];
    foreach (string path in filePaths)
    {
      if (File.Exists(path))
      {
        File.Delete(path);
      }
    }
    GC.SuppressFinalize(this);
  }
}
