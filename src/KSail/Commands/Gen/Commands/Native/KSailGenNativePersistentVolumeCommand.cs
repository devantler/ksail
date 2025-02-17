
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativePersistentVolumeCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly OutputOption _outputOption = new("./persistent-volume.yaml");
  readonly KSailGenNativePersistentVolumeCommandHandler _handler = new();
  public KSailGenNativePersistentVolumeCommand() : base("persistent-volume", "Generate a 'core/v1/PersistentVolume' resource.")
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
        catch (Exception ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
