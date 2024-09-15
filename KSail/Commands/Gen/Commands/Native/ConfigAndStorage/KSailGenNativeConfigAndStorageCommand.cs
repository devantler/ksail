
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageCommand : Command
{
  public KSailGenNativeConfigAndStorageCommand(IConsole? console = default) : base("config-and-storage", "Generate a native Kubernetes resource from the config-and-storage category.")
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
