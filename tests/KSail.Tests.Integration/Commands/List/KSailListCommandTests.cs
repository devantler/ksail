using KSail.CLIWrappers;
using KSail.Commands.Up;

namespace KSail.Tests.Integration.Commands.List;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailListCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail up [name]</c> command succeeds and creates a cluster.
  /// </summary>
  [Fact]
  public async void KSailList_SucceedsAndListsClusters()
  {
    Console.WriteLine($"🧪 Running {nameof(KSailList_SucceedsAndListsClusters)} test...");
    //Arrange
    //Act
    string clusters = await K3dCLIWrapper.ListClustersAsync();

    //Assert
    _ = await Verify(clusters);
  }

  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
}
