
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterIPAddressCommand : Command
{
  readonly FileOutputOption _outputOption = new("./ip-address.yaml");
  readonly KSailGenNativeClusterIPAddressCommandHandler _handler = new();
  public KSailGenNativeClusterIPAddressCommand() : base("ip-address", "Generate a 'networking.k8s.io/v1beta1/IPAddress' resource.")
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
