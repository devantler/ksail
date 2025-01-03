using KSail.Utils;

namespace KSail.Tests.Utils;

/// <summary>
/// Tests for <see cref="ExceptionHandler"/>.
/// </summary>
public class ExceptionHandlerTests
{
  /// <summary>
  /// Tests that <see cref="ExceptionHandler.HandleException(Exception)"/> throws an exception when <see cref="ExceptionHandler.DebugMode"/> is set to <c>true</c>.
  /// </summary>
  [Fact]
  public void HandleException_DebugMode_ThrowsException()
  {
    // Arrange
    var ExceptionHandler = new ExceptionHandler
    {
      DebugMode = true
    };
    var exception = new Exception("Test exception");

    // Act & Assert
    _ = Assert.Throws<Exception>(() => ExceptionHandler.HandleException(exception));
  }

  /// <summary>
  /// Tests that <see cref="ExceptionHandler.HandleException(Exception)"/> writes to the console when <see cref="ExceptionHandler.DebugMode"/> is set to <c>false</c>.
  /// </summary>
  [Fact]
  public void HandleException_NonDebugMode_WritesToConsole()
  {
    // Arrange
    var ExceptionHandler = new ExceptionHandler();
    var innerException = new Exception("Inner exception");
    var exception = new Exception("Test exception", innerException);

    // Act
    string message = ExceptionHandler.HandleException(exception);

    // Assert
    Assert.Contains("✗ Test exception", message);
  }
}
