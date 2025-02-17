using System.CommandLine;

namespace KSail.Commands.Root.Handlers;

/// <summary>
/// Handles the KSail root command.
/// </summary>
class KSailRootCommandHandler
{
  /// <summary>
  /// Handles the KSail root command.
  /// </summary>
  /// <param name="console"></param>
  internal static bool Handle(IConsole console)
  {
    PrintIntroduction(console);
    return true;
  }

  static void PrintIntroduction(IConsole console)
  {
    string[] lines =
    [
      @"🛥️ 🐳    Welcome to KSail!    🛥️ 🐳",
      @"                                   . . .",
      @"              __/___                 :",
      @"        _____/______|             ___|____     |""\/""|",
      @"_______/_____\_______\_____     ,'        `.    \  /",
      @"\               KSail      |    |  ^        \___/  |",
      @"~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~"
    ];

    console.WriteLine(lines[0]);

    Console.ForegroundColor = ConsoleColor.Blue;
    console.WriteLine(lines[1]);

    Console.ForegroundColor = ConsoleColor.Gray;
    console.Write(lines[2][..(lines[2].IndexOf("/__") + 4)]);
    Console.ForegroundColor = ConsoleColor.Blue;
    console.WriteLine(lines[2][(lines[2].IndexOf("/__") + 4)..]);

    Console.ForegroundColor = ConsoleColor.Gray;
    console.Write(lines[3][..(lines[3].IndexOf('|') + 1)]);
    Console.ForegroundColor = ConsoleColor.Cyan;
    console.Write(lines[3][(lines[3].IndexOf('|') + 1)..(lines[3].IndexOf("_|_") + 1)]);
    Console.ForegroundColor = ConsoleColor.Blue;
    console.Write(lines[3][(lines[3].IndexOf("_|_") + 1)..(lines[3].IndexOf("_|_") + 2)]);
    Console.ForegroundColor = ConsoleColor.Cyan;
    console.WriteLine(lines[3][(lines[3].IndexOf("_|_") + 2)..]);

    Console.ForegroundColor = ConsoleColor.Gray;
    console.Write(lines[4][..lines[4].IndexOf(',')]);
    Console.ForegroundColor = ConsoleColor.Cyan;
    console.WriteLine(lines[4][lines[4].IndexOf(',')..]);

    Console.ForegroundColor = ConsoleColor.Gray;
    console.Write(lines[5][..lines[5].IndexOf("KSail")]);
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Green;
    console.Write(lines[5][lines[5].IndexOf(" KSail ")..(lines[5].IndexOf(" KSail ") + 7)]);
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Gray;
    console.Write(lines[5][(lines[5].IndexOf(" KSail ") + 8)..lines[5].IndexOf("|  ^")]);
    Console.ForegroundColor = ConsoleColor.Cyan;
    console.WriteLine(lines[5][lines[5].IndexOf("|  ^")..]);

    Console.ForegroundColor = ConsoleColor.DarkBlue;
    console.WriteLine(lines[6]);

    Console.ResetColor();
  }
}
