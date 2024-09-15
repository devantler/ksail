
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceCommand : Command
{
  public KSailGenNativeServiceCommand(IConsole? console = default) : base("service", "Generate a native Kubernetes resource from the service category.")
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
