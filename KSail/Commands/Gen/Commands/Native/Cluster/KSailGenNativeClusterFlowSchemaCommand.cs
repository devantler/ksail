
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterFlowSchemaCommand : Command
{
  readonly FileOutputOption _outputOption = new("./flow-schema.yaml");
  readonly KSailGenNativeClusterFlowSchemaCommandHandler _handler = new();
  public KSailGenNativeClusterFlowSchemaCommand() : base("flow-schema", "Generate a 'flowcontrol.apiserver.k8s.io/v1/FlowSchema' resource.")
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
