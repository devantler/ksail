
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsCommand : Command
{
  public KSailGenNativeWorkloadsCommand(IConsole? console = default) : base("workloads", "Generate a native Kubernetes resource from the workloads category.")
  {
    AddCommands();
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  static void AddCommands()
  {
  }
}
