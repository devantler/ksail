using KSail.Utils;

namespace KSail.Tests.Utils;


[Collection("KSail.Tests")]
public class SopsConfigLoaderTest
{

  /// <returns></returns>
  [Fact]
  public async Task LoadAsync_NoSopsYamlFile_ThrowsKSailException()
  {
    // Act
    var exception = await Assert.ThrowsAsync<KSailException>(() => SopsConfigLoader.LoadAsync());

    // Assert
    Assert.Equal("'.sops.yaml' file not found in the current or parent directories", exception.Message);
  }
}
