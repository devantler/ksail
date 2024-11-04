namespace KSail;

/// <summary>
/// A simple exception handler that can be used to handle exceptions in a consistent manner.
/// </summary>
public static class ExceptionHandler
{
  /// <summary>
  /// Gets or sets a value indicating whether the application is running in debug mode.
  /// </summary>
  public static bool DebugMode { get; set; }

  /// <summary>
  /// Handles an exception by either throwing it or writing it's messages to the console.
  /// </summary>
  /// <param name="ex"></param>
  public static void HandleException(Exception ex)
  {
    if (DebugMode)
    {
      throw ex;
    }
    else
    {
      if (ex is null)
      {
        return;
      }
      Console.WriteLine($"✕ {ex.Message}");
      if (ex.InnerException is not null)
      {
        Console.WriteLine($"  {ex.InnerException.Message}");
      }
    }
  }
}
