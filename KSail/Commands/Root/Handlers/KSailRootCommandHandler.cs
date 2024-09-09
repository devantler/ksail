using System.CommandLine;
using Spectre.Console;

namespace KSail.Commands.Root.Handlers;

static class KSailRootCommandHandler
{
  public static void Handle(IConsole? console = null) => PrintIntroduction(console);

  static void PrintIntroduction(IConsole? console = null)
  {
    if (console is null)
    {
      AnsiConsole.Markup(Introduction);
    }
    else
    {
      console.WriteLine(Introduction);
    }
  }

  const string Introduction = """
    🛥️ 🐳    [bold underline]Welcome to [blue]KSail[/]![/]    🛥️ 🐳
                                         [blue]. . .[/]
                    __/___                 [blue]:[/]
              _____/______|             ___[blue]|[/]____     |"\/"|
      _______/_____\_______\_____     ,'        `.    \  /
      \               [italic]KSail[/]      |    |  ^        \___/  |
    [bold blue]~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^[/]

    """;
}
