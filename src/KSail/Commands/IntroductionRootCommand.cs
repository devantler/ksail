using System.CommandLine;
using Spectre.Console;

namespace KSail.Commands;

public class IntroductionRootCommand : RootCommand
{
  public IntroductionRootCommand() : base("KSail is a CLI tool for provisioning GitOps enabled K8s clusters in Docker.")
  {
    AddCommand(new InstallCommand());
    AddCommand(new UpCommand());
    AddCommand(new DownCommand());
    AddCommand(new UpdateCommand());
    AddCommand(new SOPSCommand());
    AddCommand(new ValidateCommand());
    AddCommand(new VerifyCommand());

    this.SetHandler(() =>
    {
      AnsiConsole.Markup("""
      🐳⛴️    [bold underline]Welcome to [blue]KSail[/]![/]    ⛴️ 🐳
                                            [blue]. . .[/]
                      __/___                 [blue]:[/]
                _____/______|             ___[blue]|[/]____     |"\/"|
        _______/_____\_______\_____     ,'        `.    \  /
        \               [italic]KSail[/]      |    |  ^        \___/  |
      [bold blue]~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^[/]


      """);

      _ = this.InvokeAsync("--help");
      return;
    });
  }
}
