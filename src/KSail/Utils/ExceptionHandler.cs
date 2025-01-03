using System.Text;

namespace KSail.Utils;

/// <summary>
/// A simple exception handler that can be used to handle exceptions in a consistent manner.
/// </summary>
public class ExceptionHandler
{
  /// <summary>
  /// Gets or sets a value indicating whether the application is running in debug mode.
  /// </summary>
  public bool DebugMode { get; set; }

  /// <summary>
  /// Handles an exception by either throwing it or writing it's messages to the console.
  /// </summary>
  /// <param name="ex"></param>
  public string HandleException(Exception ex)
  {
    if (DebugMode)
    {
      throw ex;
    }
    else
    {
      var message = new StringBuilder();
      message = message.AppendLine($"✗ {ex.Message}");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"✗ {ex.Message}");
      for (var inner = ex.InnerException; inner is not null; inner = inner.InnerException)
      {
        Console.WriteLine($"  {inner.Message}");
        message = message.AppendLine($"  {inner.Message}");
      }
      Console.ResetColor();
      return message.ToString();
    }
  }
}
