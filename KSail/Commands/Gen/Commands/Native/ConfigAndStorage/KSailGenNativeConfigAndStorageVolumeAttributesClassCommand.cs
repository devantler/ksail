
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;
using KSail.Utils;

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
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ Generating {outputFile}");
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
