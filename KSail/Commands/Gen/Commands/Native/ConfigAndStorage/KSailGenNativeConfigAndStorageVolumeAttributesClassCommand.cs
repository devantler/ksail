
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageVolumeAttributesClassCommand : Command
{
  readonly FileOutputOption _outputOption = new("./volume-attributes-class.yaml");
  readonly KSailGenNativeConfigAndStorageVolumeAttributesClassCommandHandler _handler = new();
  public KSailGenNativeConfigAndStorageVolumeAttributesClassCommand() : base("volume-attributes-class", "Generate a 'storage.k8s.io/v1beta1/VolumeAttributesClass' resource.")
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
