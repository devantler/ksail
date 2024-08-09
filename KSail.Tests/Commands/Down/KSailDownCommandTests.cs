using System.CommandLine;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KSail.Commands.Down;

namespace KSail.Tests.Commands.Down;

/// <summary>
/// Tests for the <see cref="KSailDownCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailDownCommandTests : IAsyncLifetime
{
  readonly ResourceManager ResourceManager = new("KSail.Tests.Commands.Down.Resources", Assembly.GetExecutingAssembly());
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail down</c> command fails and prints help.
  /// </summary>
  [Fact]
  public async Task KSailDown_FailsAndPrintsHelp()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailDown_FailsAndPrintsHelp), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailDownCommand = new KSailDownCommand();

    //Act
    int exitCode = await ksailDownCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }
}
