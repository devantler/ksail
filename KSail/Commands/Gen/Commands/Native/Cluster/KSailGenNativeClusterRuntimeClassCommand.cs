
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterRuntimeClassCommand : Command
{
  readonly FileOutputOption _outputOption = new("./runtime-class.yaml");
  readonly KSailGenNativeClusterRuntimeClassCommandHandler _handler = new();
  public KSailGenNativeClusterRuntimeClassCommand() : base("runtime-class", "Generate a 'node.k8s.io/v1/RuntimeClass' resource.")
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
