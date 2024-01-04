using System.Text.RegularExpressions;

namespace KSail.Utils;

/// <summary>
/// Utility methods not provided by the <see cref="Console"/> class.
/// </summary>
public static class ConsoleUtils
{
  /// <summary>
  /// Prompts the user for input.
  /// </summary>
  /// <param name="prompt">The prompt to display to the user. A '✍🏻' is prepended to the prompt, and a ':' is appended to the prompt.</param>
  /// <param name="default">The default value to use if the user does not provide input.</param>
  /// <param name="filter">A filter to apply to the user's input.</param>
  /// <returns>The user's input.</returns>
  public static string Prompt(string prompt, string @default = "", Regex? filter = null)
  {
    bool first = true;
    string? input = "";
    do
    {
      if (!first)
      {
        Console.WriteLine($"❌ Invalid input '{input}'. Please try again.");
        Console.WriteLine();
      }
      Console.WriteLine($"✍️ {prompt}:");
      Console.Write("> ");
      input = Console.ReadLine() ?? @default;
      first = false;
    }
    while (filter != null ? !filter.IsMatch(input) : string.IsNullOrWhiteSpace(input));
    Console.WriteLine();
    return input;
  }
}
