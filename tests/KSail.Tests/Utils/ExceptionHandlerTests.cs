using KSail.Utils;

namespace KSail.Tests.Utils;


public class ExceptionHandlerTests
{

  [Fact]
  public void HandleException_DebugMode_ThrowsException()
  {
    // Arrange
    var ExceptionHandler = new ExceptionHandler
    {
      DebugMode = true
    };
    var exception = new KSailException("Test exception");

    // Act & Assert
    _ = Assert.Throws<KSailException>(() => ExceptionHandler.HandleException(exception));
  }


  [Fact]
  public void HandleException_NonDebugMode_WritesToConsole()
  {
    // Arrange
    var ExceptionHandler = new ExceptionHandler();
    var innerException = new KSailException("Inner exception");
    var exception = new KSailException("Test exception", innerException);

    // Act
    string message = ExceptionHandler.HandleException(exception);

    // Assert
    Assert.Contains("✗ Test exception", message, StringComparison.Ordinal);
  }
}
