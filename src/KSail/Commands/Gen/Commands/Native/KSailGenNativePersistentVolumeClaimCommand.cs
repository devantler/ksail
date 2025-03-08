
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativePersistentVolumeClaimCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./persistent-volume-claim.yaml");
  public KSailGenNativePersistentVolumeClaimCommand() : base("persistent-volume-claim", "Generate a 'core/v1/PersistentVolumeClaim' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          if (overwrite)
          {
            Console.WriteLine($"✚ overwriting {outputFile}");
          }
          else
          {
            Console.WriteLine($"✚ generating {outputFile}");
          }
          KSailGenNativePersistentVolumeClaimCommandHandler handler = new(outputFile, overwrite);
          context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
