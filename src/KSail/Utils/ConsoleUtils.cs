using System.Globalization;
using System.Text.RegularExpressions;

namespace KSail.Utils;

static class ConsoleUtils
{
  internal static string Prompt(string prompt, string @default = "", Regex? filter = null)
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

  internal static bool PromptLogin() =>
    //TODO: Implement login
    true;
}
