
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceCommand : Command
{
  public KSailGenNativeServiceCommand(IConsole? console = default) : base("services", "Generate a native Kubernetes resource from the service category.")
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
    AddCommand(new KSailGenNativeServiceIngressClassCommand());
    AddCommand(new KSailGenNativeServiceIngressCommand());
    AddCommand(new KSailGenNativeServiceServiceCommand());
  }
}
