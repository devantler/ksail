
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Flux;

class KSailGenFluxCommand : Command
{
  public KSailGenFluxCommand(IConsole? console = default) : base("flux", "Generate a Flux resource.")
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
    AddCommand(new KSailGenFluxHelmReleaseCommand());
    AddCommand(new KSailGenFluxHelmRepositoryCommand());
    AddCommand(new KSailGenFluxKustomizationCommand());
  }
}
