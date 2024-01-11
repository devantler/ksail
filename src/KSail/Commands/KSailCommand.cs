using System.CommandLine;
using KSail.Commands.Check;
using KSail.Commands.Down;
using KSail.Commands.Lint;
using KSail.Commands.List;
using KSail.Commands.SOPS;
using KSail.Commands.Up;
using KSail.Commands.Update;
using Spectre.Console;

namespace KSail.Commands;

sealed class KSailCommand : RootCommand
{
  internal KSailCommand() : base("KSail is a CLI tool for provisioning GitOps enabled K8s clusters in Docker.")
  {
    AddCommand(new KSailUpCommand());
    AddCommand(new KSailDownCommand());
    AddCommand(new KSailUpdateCommand());
    AddCommand(new KSailListCommand());
    AddCommand(new KSailLintCommand());
    AddCommand(new KSailCheckCommand());
    AddCommand(new KSailSOPSCommand());

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
