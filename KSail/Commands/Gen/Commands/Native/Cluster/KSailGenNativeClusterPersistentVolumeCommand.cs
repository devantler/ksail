
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterPersistentVolumeCommand : Command
{
  readonly FileOutputOption _outputOption = new("./persistent-volume.yaml");
  readonly KSailGenNativeClusterPersistentVolumeCommandHandler _handler = new();
  public KSailGenNativeClusterPersistentVolumeCommand() : base("persistent-volume", "Generate a 'core/v1/PersistentVolume' resource.")
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
