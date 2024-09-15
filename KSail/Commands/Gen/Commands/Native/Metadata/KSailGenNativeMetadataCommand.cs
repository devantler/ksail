
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataCommand : Command
{
  public KSailGenNativeMetadataCommand(IConsole? console = default) : base("metadata", "Generate a native Kubernetes resource from the metadata category.")
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
