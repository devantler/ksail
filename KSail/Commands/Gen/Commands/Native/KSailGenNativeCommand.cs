
using System.CommandLine;
using KSail.Commands.Gen.Commands.Native.Cluster;
using KSail.Commands.Gen.Commands.Native.ConfigAndStorage;
using KSail.Commands.Gen.Commands.Native.Metadata;
using KSail.Commands.Gen.Commands.Native.Service;
using KSail.Commands.Gen.Commands.Native.Workloads;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativeCommand : Command
{

  public KSailGenNativeCommand(IConsole? console = default) : base("native", "Generate a native Kubernetes resource from one of the available categories.")
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
    AddCommand(new KSailGenNativeClusterCommand(console));
    AddCommand(new KSailGenNativeConfigAndStorageCommand(console));
    AddCommand(new KSailGenNativeMetadataCommand(console));
    AddCommand(new KSailGenNativeServiceCommand(console));
    AddCommand(new KSailGenNativeWorkloadsCommand(console));
  }
}
