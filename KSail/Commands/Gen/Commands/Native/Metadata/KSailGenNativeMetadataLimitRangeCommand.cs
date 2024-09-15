
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

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
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
