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
      @"                    __ ______     _ __",
      @"                   / //_/ __/__ _(_) /",
      @"                  / ,< _\ \/ _ `/ / /",
      @"                 /_/|_/___/\_,_/_/_/",
      @"                                   . . .",
      @"              __/___                 :",
      @"        _____/______|             ___|____     |""\/""|",
      @"_______/_____\_______\_____     ,'        `.    \  /",
      @"\   -----       -\-\-\-    |    |  ^        \___/  |",
      @"~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~",
      @""
    ];

    Console.ForegroundColor = ConsoleColor.Yellow;
    console.WriteLine(lines[0]);
    console.WriteLine(lines[1]);
    console.WriteLine(lines[2]);
    console.WriteLine(lines[3]);

    Console.ForegroundColor = ConsoleColor.Blue;
    console.WriteLine(lines[4]);
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    console.Write(lines[5][..(lines[5].IndexOf("/__") + 4)]);
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    console.WriteLine(lines[5][(lines[5].IndexOf("/__") + 4)..]);


    Console.ForegroundColor = ConsoleColor.DarkGreen;
    console.Write(lines[6][..(lines[6].IndexOf('|') + 1)]);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    console.Write(lines[6][(lines[6].IndexOf('|') + 1)..(lines[6].IndexOf("_|_") + 1)]);
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    console.Write(lines[6][(lines[6].IndexOf("_|_") + 1)..(lines[6].IndexOf("_|_") + 2)]);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    console.WriteLine(lines[6][(lines[6].IndexOf("_|_") + 2)..]);

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    console.Write(lines[7][..lines[7].IndexOf(',')]);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    console.WriteLine(lines[7][lines[7].IndexOf(',')..]);


    Console.ForegroundColor = ConsoleColor.DarkGreen;
    console.Write(lines[8][..lines[8].IndexOf("|  ^")]);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    console.WriteLine(lines[8][lines[8].IndexOf("|  ^")..]);

    Console.ForegroundColor = ConsoleColor.DarkBlue;
    console.WriteLine(lines[9]);
    console.WriteLine(lines[10]);

    Console.ResetColor();
  }
}
