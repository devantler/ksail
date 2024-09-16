
using System.CommandLine;
using KSail.Commands.Gen.Commands.Config;
using KSail.Commands.Gen.Commands.Flux;
using KSail.Commands.Gen.Commands.Kustomize;
using KSail.Commands.Gen.Commands.Native;

namespace KSail.Commands.Gen;

sealed class KSailGenCommand : Command
{
  internal KSailGenCommand(IConsole? console = default) : base("gen", "Generate a resource.")
  {
    AddCommands(console);
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands(IConsole? console)
  {
    AddCommand(new KSailGenConfigCommand(console));
    AddCommand(new KSailGenFluxCommand(console));
    AddCommand(new KSailGenKustomizeCommand(console));
    AddCommand(new KSailGenNativeCommand(console));
  }
}


