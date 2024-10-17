using System.CommandLine;
using KSail.Commands.Gen;
using KSail.Commands.Init;
using KSail.Commands.Root.Handlers;

namespace KSail.Commands.Root;

sealed class KSailRootCommand : RootCommand
{
  internal KSailRootCommand(IConsole? console = null) : base("KSail is an SDK for GitOps enabled clusters.")
  {
    AddCommands(console);

    this.SetHandler(async (context) =>
      {
        KSailRootCommandHandler.Handle(console);
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands(IConsole? console)
  {
    AddCommand(new KSailInitCommand());
    AddCommand(new KSailGenCommand(console));
    // AddCommand(new KSailCheckCommand());
    // AddCommand(new KSailDebugCommand());
    // AddCommand(new KSailDownCommand());
    // AddCommand(new KSailLintCommand());
    // AddCommand(new KSailListCommand());
    // AddCommand(new KSailSOPSCommand());
    // AddCommand(new KSailStartCommand());
    // AddCommand(new KSailStopCommand());
    // AddCommand(new KSailUpCommand());
    // AddCommand(new KSailUpdateCommand());
  }
}
