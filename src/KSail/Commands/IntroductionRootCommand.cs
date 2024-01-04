using System.CommandLine;
using Spectre.Console;

namespace KSail.Commands;

/// <summary>
/// The root command responsible for setting up the KSail CLI entrypoint.
/// </summary>
public class IntroductionRootCommand : RootCommand
{
  /// <summary>
  /// Initializes a new instance of the <see cref="IntroductionRootCommand"/> class.
  /// </summary>
  public IntroductionRootCommand() : base("KSail is a CLI tool for provisioning GitOps enabled K8s clusters .")
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
    });
  }
}
