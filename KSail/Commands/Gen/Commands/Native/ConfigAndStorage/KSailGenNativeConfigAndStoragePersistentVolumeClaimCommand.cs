
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;
using KSail.Utils;

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
