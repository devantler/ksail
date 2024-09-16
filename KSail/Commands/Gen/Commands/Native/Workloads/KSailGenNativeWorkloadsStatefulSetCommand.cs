
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsStatefulSetCommand : Command
{
  readonly FileOutputOption _outputOption = new("./stateful-set.yaml");
  readonly KSailGenNativeWorkloadsStatefulSetCommandHandler _handler = new();
  public KSailGenNativeWorkloadsStatefulSetCommand() : base("stateful-set", "Generate a 'apps/v1/StatefulSet' resource.")
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
