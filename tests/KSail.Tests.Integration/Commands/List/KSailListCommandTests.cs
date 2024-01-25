using KSail.CLIWrappers;
using KSail.Commands.Up;
using KSail.Tests.Integration.TestUtils;

namespace KSail.Tests.Integration.Commands.List;

/// <summary>
/// Tests for the <see cref="KSailUpCommand"/> class.
/// </summary>
[Collection("KSail.Tests.Integration")]
public class KSailListCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public async Task InitializeAsync() => await KSailTestUtils.Cleanup();

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
  public async Task DisposeAsync() => await KSailTestUtils.Cleanup();
}
