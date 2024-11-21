
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigCommand : Command
{
  public KSailGenConfigCommand(IConsole? console = default) : base("config", "Generate a configuration file.")
  {
    AddCommands();
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailGenConfigK3dCommand());
    AddCommand(new KSailGenConfigKSailCommand());
    AddCommand(new KSailGenConfigSOPSCommand());
  }
}


