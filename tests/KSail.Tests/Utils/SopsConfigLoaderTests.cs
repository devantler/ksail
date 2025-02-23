using KSail.Utils;

namespace KSail.Tests.Utils;

/// <summary>
/// Tests for <see cref="SopsConfigLoader"/>.
/// </summary>
public class SopsConfigLoaderTest
{
  /// <summary>
  /// Test that <see cref="SopsConfigLoader.LoadAsync"/> throws <see cref="KSailException"/> when no '.sops.yaml' file is found.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task LoadAsync_NoSopsYamlFile_ThrowsKSailException()
  {
    // Arrange
    string testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    _ = Directory.CreateDirectory(testDirectory);

    try
    {
      // Act & Assert
      var exception = await Assert.ThrowsAsync<KSailException>(() => SopsConfigLoader.LoadAsync(testDirectory));
      Assert.Equal("'.sops.yaml' file not found in the current or parent directories", exception.Message);
    }
    finally
    {
      // Cleanup
      Directory.Delete(testDirectory, true);
    }
  }
}
