
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsDaemonSetCommand : Command
{
  readonly FileOutputOption _outputOption = new("./daemon-set.yaml");
  readonly KSailGenNativeWorkloadsDaemonSetCommandHandler _handler = new();
  public KSailGenNativeWorkloadsDaemonSetCommand() : base("daemon-set", "Generate a 'apps/v1/DaemonSet' resource.")
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
