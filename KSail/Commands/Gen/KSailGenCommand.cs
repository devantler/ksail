
using System.CommandLine;
using KSail.Commands.Gen.Commands;
using KSail.Commands.Gen.Handlers;

namespace KSail.Commands.Gen;

sealed class KSailGenCommand : Command
{
  readonly KSailGenCommandHandler _handler = new();
  internal KSailGenCommand(IConsole? console = default) : base("gen", "Generate a resource.")
  {
    AddCommands();
    this.SetHandler(async (context) =>
      {
        _ = await _handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailGenFluxKustomizationCommand());
    AddCommand(new KSailGenK3dConfigCommand());
    AddCommand(new KSailGenKustomizeComponentCommand());
  }
}
