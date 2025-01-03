
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;
using KSail.Utils;

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
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
