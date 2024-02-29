using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Init;
using KSail.Commands.SOPS;

namespace KSail.Tests.Integration.Commands.SOPS;

/// <summary>
/// Tests for the <see cref="KSailSOPSCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailSOPSCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;
  /// <summary>
  /// Tests that the 'ksail sops' command fails and prints the help text.
  /// </summary>
  [Fact]
  public async Task KSailSOPS_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPS_FailsAndPrintsHelp)}");
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
  /// Tests that the 'ksail sops ksail --show-key' command prints the full key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSShowKey_PrintsFullKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSShowKey_PrintsFullKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("ksail --show-key");

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --show-public-key' command prints the public key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSShowPublicKey_PrintsPublicKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSShowPublicKey_PrintsPublicKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("ksail --show-public-key");

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --show-private-key' command prints the private key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSShowPrivateKey_PrintsPrivateKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSShowPrivateKey_PrintsPrivateKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("ksail --show-private-key");

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --import [key]' command imports the key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSImportKey_ImportsKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSImportKey_ImportsKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    string key = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey"));
    int exitCode = await ksailSOPSCommand.InvokeAsync($"ksail --import \"{key}\"");

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --import [keyPath]' command imports the key.
  /// </summary>
  [Fact]
  public async Task KSailSOPSImportKeyFromFile_ImportsKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSImportKeyFromFile_ImportsKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync($"ksail --import {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")}");

    //Assert
    Assert.Equal(0, exitCode);
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --export [path]' command exports the key to the specified path.
  /// </summary>
  [Fact]
  public async Task KSailSOPSExportKey_ExportsKey()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSExportKey_ExportsKey)}");
    //Arrange
    var ksailSOPSCommand = new KSailSOPSCommand();

    //Act
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", "ksail.agekey")))
    {
      _ = await ksailSOPSCommand.InvokeAsync("ksail --generate-key");
    }
    int exitCode = await ksailSOPSCommand.InvokeAsync("ksail --export ./");

    //Assert
    Assert.Equal(0, exitCode);
    Assert.True(File.Exists("./ksail.agekey"));
    string key = await File.ReadAllTextAsync("./ksail.agekey");
    Assert.NotEmpty(key);

    //Cleanup
    File.Delete("./ksail.agekey");
  }

  /// <summary>
  /// Tests that the 'ksail sops ksail --encrypt [path]' and 'ksail sops --decrypt [path]' commands successfully encrypts and decrypts a file.
  /// </summary>
  [Fact]
  public async Task KSailSOPSEncryptAndDecrypt_SuccessfullyEncryptsAndDecryptsFile()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailSOPSEncryptAndDecrypt_SuccessfullyEncryptsAndDecryptsFile)}");
    // Arrange
    var ksailInitCommand = new KSailInitCommand();
    var ksailSOPSCommand = new KSailSOPSCommand();

    // Act
    int initExitCode = await ksailInitCommand.InvokeAsync("ksail");
    int encryptExitCode = await ksailSOPSCommand.InvokeAsync("ksail --encrypt k8s/clusters/ksail/variables/variables-sensitive.sops.yaml");
    int decryptExitCode = await ksailSOPSCommand.InvokeAsync("ksail --decrypt k8s/clusters/ksail/variables/variables-sensitive.sops.yaml");

    // Assert
    Assert.Equal(0, initExitCode);
    Assert.Equal(0, encryptExitCode);
    Assert.Equal(0, decryptExitCode);
  }
}
