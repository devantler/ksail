using System.CommandLine;
using KSail.Commands.Debug;
using KSail.Commands.Down;
using KSail.Commands.Gen;
using KSail.Commands.Init;
using KSail.Commands.Lint;
using KSail.Commands.List;
using KSail.Commands.Root.Handlers;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;
using KSail.Commands.Update;
using KSail.Utils;

namespace KSail.Commands.Root;

sealed class KSailRootCommand : RootCommand
{
  readonly ExceptionHandler _exceptionHandler = new();
  internal KSailRootCommand(IConsole console) : base("KSail is an SDK for building GitOps enabled clusters.")
  {
    AddCommands(console);

    this.SetHandler(async (context) =>
      {
        try
        {
          bool exitCode = KSailRootCommandHandler.Handle(console) && await this.InvokeAsync("--help", console).ConfigureAwait(false) == 0;
          context.ExitCode = exitCode ? 0 : 1;
        }
        catch (OperationCanceledException ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }

  void AddCommands(IConsole console)
  {
    AddCommand(new KSailInitCommand());
    AddCommand(new KSailLintCommand());
    AddCommand(new KSailListCommand());
    AddCommand(new KSailUpCommand());
    AddCommand(new KSailDownCommand());
    AddCommand(new KSailUpdateCommand());
    AddCommand(new KSailStartCommand());
    AddCommand(new KSailStopCommand());
    AddCommand(new KSailDebugCommand());
    AddCommand(new KSailGenCommand(console));
    // AddCommand(new KSailSecCommand());
  }
}
