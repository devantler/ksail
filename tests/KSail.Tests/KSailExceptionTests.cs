namespace KSail.Tests;

/// <summary>
/// Tests for the <see cref="KSailException"/> class.
/// </summary>
public class KSailExceptionTests
{
  /// <summary>
  /// Tests the default constructor of the <see cref="KSailException"/> class.
  /// </summary>
  [Fact]
  public void KSailException_DefaultConstructor_ShouldCreateInstance()
  {
    // Act
    var exception = new KSailException();

    // Assert
    Assert.NotNull(exception);
    _ = Assert.IsType<KSailException>(exception);
  }

  /// <summary>
  /// Tests the constructor of the <see cref="KSailException"/> class with a message.
  /// </summary>
  [Fact]
  public void KSailException_ConstructorWithMessage_ShouldSetMessage()
  {
    // Arrange
    string message = "Test message";

    // Act
    var exception = new KSailException(message);

    // Assert
    Assert.NotNull(exception);
    Assert.Equal(message, exception.Message);
  }

  /// <summary>
  /// Tests the constructor of the <see cref="KSailException"/> class with a message and an inner exception.
  /// </summary>
  [Fact]
  public void KSailException_ConstructorWithMessageAndInnerException_ShouldSetMessageAndInnerException()
  {
    // Arrange
    string message = "Test message";
    var innerException = new Exception("Inner exception");

    // Act
    var exception = new KSailException(message, innerException);

    // Assert
    Assert.NotNull(exception);
    Assert.Equal(message, exception.Message);
    Assert.Equal(innerException, exception.InnerException);
  }
}
