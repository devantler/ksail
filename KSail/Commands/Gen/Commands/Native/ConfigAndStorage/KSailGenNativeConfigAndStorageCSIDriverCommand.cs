
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageCSIDriverCommand : Command
{
  readonly FileOutputOption _outputOption = new("./csi-driver.yaml");
  readonly KSailGenNativeConfigAndStorageCSIDriverCommandHandler _handler = new();
  public KSailGenNativeConfigAndStorageCSIDriverCommand() : base("csi-driver", "Generate a 'storage.k8s.io/v1/CSIDriver' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
