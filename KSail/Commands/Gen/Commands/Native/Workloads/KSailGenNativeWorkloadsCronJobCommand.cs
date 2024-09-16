
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsCronJobCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cron-job.yaml");
  readonly KSailGenNativeWorkloadsCronJobCommandHandler _handler = new();
  public KSailGenNativeWorkloadsCronJobCommand() : base("cron-job", "Generate a 'batch/v1/CronJob' resource.")
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
