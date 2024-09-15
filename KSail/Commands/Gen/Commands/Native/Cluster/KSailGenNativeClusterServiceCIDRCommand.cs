
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterServiceCIDRCommand : Command
{
  readonly FileOutputOption _outputOption = new("./service-cidr.yaml");
  readonly KSailGenNativeClusterServiceCIDRCommandHandler _handler = new();
  public KSailGenNativeClusterServiceCIDRCommand() : base("service-cidr", "Generate a 'networking.k8s.io/v1beta1/ServiceCIDR' resource.")
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
