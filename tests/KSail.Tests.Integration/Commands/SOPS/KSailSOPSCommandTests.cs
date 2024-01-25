using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.SOPS;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.SOPS;

/// <summary>
/// Tests for the <see cref="KSailSOPSCommand"/> class.
/// </summary>
[Collection("KSail Tests Collection")]
public class KSailSOPSCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => KSailTestUtils.CleanupAsync();

  /// <summary>
  /// Tests that the 'ksail sops' command fails and prints the help text.
  /// </summary>
  [Fact]
  public async Task KSailSOPS_FailsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    int exitCode = await ksailSOPSCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail sops --show-public-key' command prints the public key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSShowPublicKey_PrintsPublicKey()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("--show-public-key", console);

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops --show-private-key' command prints the private key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSShowPrivateKey_PrintsPrivateKey()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("--show-private-key", console);

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops --import [key]' command imports the key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSImportKey_ImportsKey()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    string key = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"));
    int exitCode = await ksailSOPSCommand.InvokeAsync($"--import \"{key}\"", console);

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops --import [keyPath]' command imports the key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSImportKeyFromFile_ImportsKey()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync($"--import {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")}", console);

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops --export [path]' command exports the key to the specified path.
  /// </summary>
  [Fact]
  public async Task KSailSOPSExportKey_ExportsKey()
  {
    //Arrange
    var console = new TestConsole();
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("--generate-key", console);
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("--export ./", console);

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(File.Exists("./ksail_sops.agekey"));
    string key = await File.ReadAllTextAsync("./ksail_sops.agekey");
    Assert.NotEmpty(key);

    //Cleanup
    File.Delete("./ksail_sops.agekey");
  }
}