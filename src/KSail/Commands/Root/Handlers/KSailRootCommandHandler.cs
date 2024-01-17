using System.CommandLine;
using Spectre.Console;

namespace KSail.Commands.Root.Handlers;

internal static class KSailRootCommandHandler
{
  public static void Handle(IConsole? console = null) => PrintIntroduction(console);

  private static void PrintIntroduction(IConsole? console = null)
  {
    if (console is null)
    {
      AnsiConsole.Markup(introduction);
    }
    else
    {
      console.WriteLine(introduction);
    }
  }

  private const string introduction = """
    🐳⛴️    [bold underline]Welcome to [blue]KSail[/]![/]    ⛴️ 🐳
                                         [blue]. . .[/]
                    __/___                 [blue]:[/]
              _____/______|             ___[blue]|[/]____     |"\/"|
      _______/_____\_______\_____     ,'        `.    \  /
      \               [italic]KSail[/]      |    |  ^        \___/  |
    [bold blue]~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^[/]

    """;
}
