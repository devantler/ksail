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
  readonly IConsole console;
  internal KSailCommand(IConsole console) : base("KSail is a CLI tool for provisioning GitOps enabled K8s clusters in Docker.")
  {
    this.console = console;
    AddCommand(new KSailUpCommand(console));
    AddCommand(new KSailDownCommand(console));
    AddCommand(new KSailUpdateCommand(console));
    AddCommand(new KSailListCommand(console));
    AddCommand(new KSailLintCommand(console));
    AddCommand(new KSailCheckCommand(console));
    AddCommand(new KSailSOPSCommand(console));

    this.SetHandler(() =>
    {
      Introduction();
      _ = this.InvokeAsync("--help", console);
    });
  }

  void Introduction()
  {
    if (console is not null)
    {
      console.WriteLine(introduction);
    }
    else
    {
      AnsiConsole.WriteLine(introduction);
    }
  }

  const string introduction = """
    🐳⛴️    [bold underline]Welcome to [blue]KSail[/]![/]    ⛴️ 🐳
                                         [blue]. . .[/]
                    __/___                 [blue]:[/]
              _____/______|             ___[blue]|[/]____     |"\/"|
      _______/_____\_______\_____     ,'        `.    \  /
      \               [italic]KSail[/]      |    |  ^        \___/  |
    [bold blue]~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^[/]

    """;
}
