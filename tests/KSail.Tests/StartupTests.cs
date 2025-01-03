using System.Runtime.InteropServices;

namespace KSail.Tests;

/// <summary>
/// Provides a set of tests for the <see cref="Startup" /> class.
/// </summary>
public class StartupTests
{
  /// <summary>
  /// Tests that the <see cref="Startup.RunAsync(string[])" /> method behaves correctly on Windows operating systems.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task RunAsync_ShouldReturnOne_OnWindows()
  {
    // Arrange
    Console.SetOut(Console.Out);
    Environment.SetEnvironmentVariable("WINDOWS_TEST", "true");
    var startup = new Startup();

    // Act
    int result = await startup.RunAsync([]);

    // Assert
    Assert.Equal(1, result);

    // Cleanup
    Environment.SetEnvironmentVariable("WINDOWS_TEST", null);
  }


  /// <summary>
  /// Tests that the <see cref="Startup.RunAsync(string[])" /> method behaves correctly on non-Windows operating systems.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task RunAsync_ShouldReturnZero_OnNonWindows()
  {
    // Arrange
    var startup = new Startup();

    // Act
    int result = await startup.RunAsync([]);

    // Assert
    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Assert.Equal(0, result);
    }
  }
}
