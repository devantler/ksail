
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsJobCommand : Command
{
  readonly FileOutputOption _outputOption = new("./job.yaml");
  readonly KSailGenNativeWorkloadsJobCommandHandler _handler = new();
  public KSailGenNativeWorkloadsJobCommand() : base("job", "Generate a 'batch/v1/Job' resource.")
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
