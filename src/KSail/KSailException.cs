namespace KSail;

/// <summary>
/// Represents an exception that is thrown when an error occurs in KSail.
/// </summary>
[Serializable]
public class KSailException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="KSailException"/> class.
  /// </summary>
  public KSailException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailException"/> class with a specified error message.
  /// </summary>
  /// <param name="message"></param>
  public KSailException(string? message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public KSailException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
