
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Kustomize;

class KSailGenKustomizeCommand : Command
{
  public KSailGenKustomizeCommand(IConsole? console = default) : base("kustomize", "Generate a Kustomize resource.")
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
    AddCommand(new KSailGenKustomizeComponentCommand());
    AddCommand(new KSailGenKustomizeKustomizationCommand());
  }
}
