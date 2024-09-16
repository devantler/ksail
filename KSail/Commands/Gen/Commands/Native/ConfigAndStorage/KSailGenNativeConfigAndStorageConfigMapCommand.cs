
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageConfigMapCommand : Command
{
  readonly FileOutputOption _outputOption = new("./config-map.yaml");
  readonly KSailGenNativeConfigAndStorageConfigMapCommandHandler _handler = new();
  public KSailGenNativeConfigAndStorageConfigMapCommand() : base("config-map", "Generate a 'core/v1/ConfigMap' resource.")
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
