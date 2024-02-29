using System.CommandLine;
using System.CommandLine.IO;
using System.Text.RegularExpressions;
using KSail.Commands.Check;

namespace KSail.Tests.Integration.Commands.Check;

/// <summary>
/// Tests for the <see cref="KSailCheckCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public partial class KSailCheckCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail check</c> command fails and prints help.
  /// </summary>
  /// <exception cref="InvalidOperationException"></exception>
  [Fact]
  public async Task KSailCheck_FailsAndPrintsHelp()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailCheck_FailsAndPrintsHelp)}");
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("", console);
    string replacement = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.kube/config";
    string? output = console.Out.ToString() ?? throw new InvalidOperationException("🚨 Console output is null");
    output = HomeFolderRegex().Replace(output, replacement);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error + output).AutoVerify();
  }

  [GeneratedRegex("/.*\\/.*\\/.kube/config")]
  private static partial Regex HomeFolderRegex();
}
