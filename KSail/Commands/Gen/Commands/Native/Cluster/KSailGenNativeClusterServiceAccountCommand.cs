
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterServiceAccountCommand : Command
{
  readonly FileOutputOption _outputOption = new("./service-account.yaml");
  readonly KSailGenNativeClusterServiceAccountCommandHandler _handler = new();
  public KSailGenNativeClusterServiceAccountCommand() : base("service-account", "Generate a 'core/v1/ServiceAccount' resource.")
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
