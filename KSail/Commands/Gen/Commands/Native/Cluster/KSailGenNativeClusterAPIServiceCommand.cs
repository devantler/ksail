
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterAPIServiceCommand : Command
{
  readonly FileOutputOption _outputOption = new("./api-service.yaml");
  readonly KSailGenNativeClusterAPIServiceCommandHandler _handler = new();
  public KSailGenNativeClusterAPIServiceCommand() : base("api-service", "Generate a 'apiregistration.k8s.io/v1/APIService' resource.")
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
