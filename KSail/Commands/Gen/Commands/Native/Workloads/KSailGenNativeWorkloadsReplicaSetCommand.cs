
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsReplicaSetCommand : Command
{
  readonly FileOutputOption _outputOption = new("./ingress-class.yaml");
  readonly KSailGenNativeWorkloadsReplicaSetCommandHandler _handler = new();
  public KSailGenNativeWorkloadsReplicaSetCommand() : base("replica-set", "Generate a 'apps/v1/ReplicaSet' resource.")
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
