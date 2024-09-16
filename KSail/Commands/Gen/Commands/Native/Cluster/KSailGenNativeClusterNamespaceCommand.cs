
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterNamespaceCommand : Command
{
  readonly FileOutputOption _outputOption = new("./namespace.yaml");
  readonly KSailGenNativeClusterNamespaceCommandHandler _handler = new();
  public KSailGenNativeClusterNamespaceCommand() : base("namespace", "Generate a 'core/v1/Namespace' resource.")
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
