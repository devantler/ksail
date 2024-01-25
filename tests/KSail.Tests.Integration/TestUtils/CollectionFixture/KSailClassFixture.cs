namespace KSail.Tests.Integration.TestUtils.CollectionFixture;

/// <summary>
/// A class fixture for KSail tests.
/// </summary>
public class KSailClassFixture : IAsyncLifetime
{
  /// <inheritdoc/>
  public async Task DisposeAsync() => await KSailTestUtils.Cleanup();
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;
}
