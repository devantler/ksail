
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataLimitRangeCommand : Command
{
  readonly FileOutputOption _outputOption = new("./limit-range.yaml");
  readonly KSailGenNativeMetadataLimitRangeCommandHandler _handler = new();

  public KSailGenNativeMetadataLimitRangeCommand() : base("limit-range", "Generate a 'core/v1/LimitRange' resource.")
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
