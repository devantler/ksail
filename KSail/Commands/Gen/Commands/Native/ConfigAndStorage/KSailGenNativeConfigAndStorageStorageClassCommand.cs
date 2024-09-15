
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageStorageClassCommand : Command
{
  readonly FileOutputOption _outputOption = new("./storage-class.yaml");
  readonly KSailGenNativeConfigAndStorageStorageClassCommandHandler _handler = new();
  public KSailGenNativeConfigAndStorageStorageClassCommand() : base("storage-class", "Generate a 'storage.k8s.io/v1/StorageClass' resource.")
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
