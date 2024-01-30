using System.CommandLine;
using KSail.Commands.Check;
using KSail.Commands.Down;
using KSail.Commands.Init;
using KSail.Commands.Lint;
using KSail.Commands.List;
using KSail.Commands.Root.Handlers;
using KSail.Commands.SOPS;
using KSail.Commands.Start;
using KSail.Commands.Stop;
using KSail.Commands.Up;
using KSail.Commands.Update;

namespace KSail.Commands.Root;

sealed class KSailRootCommand : RootCommand
{
  internal KSailRootCommand(IConsole? console = null) : base("KSail is a CLI tool for provisioning GitOps enabled K8s clusters in Docker.")
  {
    CancellationToken token = default;
    this.SetHandler(async (context) =>
      {
        token = context.GetCancellationToken();
        KSailRootCommandHandler.Handle(console);
        _ = await this.InvokeAsync("--help", console);
      }
    );
    AddCommands(token);
  }

  void AddCommands(CancellationToken token)
  {
    AddCommand(new KSailInitCommand(token));
    AddCommand(new KSailUpCommand(token));
    AddCommand(new KSailStartCommand(token));
    AddCommand(new KSailUpdateCommand(token));
    AddCommand(new KSailStopCommand(token));
    AddCommand(new KSailDownCommand(token));
    AddCommand(new KSailListCommand(token));
    AddCommand(new KSailLintCommand(token));
    AddCommand(new KSailCheckCommand(token));
    AddCommand(new KSailSOPSCommand(token));
  }
}
