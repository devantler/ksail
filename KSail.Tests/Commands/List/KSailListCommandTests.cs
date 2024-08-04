using System.CommandLine;
using KSail.Commands.List;
using KSail.Commands.Up;

namespace KSail.Tests.Commands.List;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailListCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail list' command succeeds and lists clusters.
  /// </summary>
  [Fact]
  public async Task KSailList_SucceedsAndListsClusters()
  {
    Console.WriteLine($"🧪 Running test: {nameof(KSailList_SucceedsAndListsClusters)}");
    //Arrange
    var ksailListCommand = new KSailListCommand();

    //Act
    int exitCode = await ksailListCommand.InvokeAsync("");

    //Assert
    Assert.Equal(0, exitCode);
  }
}
