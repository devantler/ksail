using System.CommandLine;
using KSail.Commands.Down;
using KSail.Commands.Up;
using KSail.Commands.Update;
using Spectre.Console;

namespace KSail.Commands;

sealed class KSailCommand : RootCommand
{
  internal KSailCommand() : base("KSail is a CLI tool for provisioning K8s clusters.")
  {
    AddCommand(new UpCommand());
    AddCommand(new DownCommand());
    AddCommand(new UpdateCommand());
    //AddCommand(new ListCommand());
    //AddCommand(new SOPSCommand());
    //AddCommand(new ValidateCommand());
    //AddCommand(new VerifyCommand());

    this.SetHandler(() =>
    {
      Introduction();
      _ = this.InvokeAsync("--help");
    });
  }

  static void Introduction() => AnsiConsole.Markup("""
    🐳⛴️    [bold underline]Welcome to [blue]KSail[/]![/]    ⛴️ 🐳
                                          [blue]. . .[/]
                    __/___                 [blue]:[/]
              _____/______|             ___[blue]|[/]____     |"\/"|
      _______/_____\_______\_____     ,'        `.    \  /
      \               [italic]KSail[/]      |    |  ^        \___/  |
    [bold blue]~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^[/]


    """);
}
