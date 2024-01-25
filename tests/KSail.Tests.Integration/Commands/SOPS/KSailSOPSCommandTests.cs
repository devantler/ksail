using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.SOPS;

namespace KSail.Tests.Integration.Commands.SOPS;

/// <summary>
/// Tests for the <see cref="KSailSOPSCommand"/> class.
/// </summary>
public class KSailSOPSCommandTests
{
  /// <summary>
  /// Tests that the 'ksail sops' command fails and prints the help text.
  /// </summary>
  [Fact]
  public async void KSailSOPS_FailsAndPrintsHelp()
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
  public async void KSailSOPSShowPublicKey_PrintsPublicKey()
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
  public async void KSailSOPSShowPrivateKey_PrintsPrivateKey()
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
}
