
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStoragePersistentVolumeClaimCommand : Command
{
  readonly FileOutputOption _outputOption = new("./persistent-volume-claim.yaml");
  readonly KSailGenNativeConfigAndStoragePersistentVolumeClaimCommandHandler _handler = new();
  public KSailGenNativeConfigAndStoragePersistentVolumeClaimCommand() : base("persistent-volume-claim", "Generate a 'core/v1/PersistentVolumeClaim' resource.")
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
