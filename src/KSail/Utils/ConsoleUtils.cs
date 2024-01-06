using System.Globalization;
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
      string yesNoText = filter?.Equals(RegexFilters.YesNoFilter()) == true ? " (y/n)" : "";
      string defaultText = @default != "" ? $" [{@default}]" : "";
      if (filter?.Equals(RegexFilters.YesNoFilter()) == true)
      {
        defaultText = @default == "true" ? " [y]" : " [n]";
      }
      Console.WriteLine($"✍️ {prompt}{yesNoText}{defaultText}:");
      Console.Write("> ");
      input = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(input))
      {
        input = @default;
      }
      else if (filter?.Equals(RegexFilters.YesNoFilter()) == true)
      {
        input = input.ToLower(CultureInfo.InvariantCulture) switch
        {
          "y" => "true",
          "n" => "false",
          _ => input
        };
      }
      first = false;
    }
    while (filter != null ? !filter.IsMatch(input) : string.IsNullOrWhiteSpace(input));
    Console.WriteLine();
    return input;
  }
}
