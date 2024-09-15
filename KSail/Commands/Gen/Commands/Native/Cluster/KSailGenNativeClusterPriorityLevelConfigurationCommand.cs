
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterPriorityLevelConfigurationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./priority-level-configuration.yaml");
  readonly KSailGenNativeClusterPriorityLevelConfigurationCommandHandler _handler = new();
  public KSailGenNativeClusterPriorityLevelConfigurationCommand() : base("priority-level-configuration", "Generate a 'flowcontrol.apiserver.k8s.io/v1/PriorityLevelConfiguration' resource.")
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
