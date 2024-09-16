
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterNetworkPolicyCommand : Command
{
  readonly FileOutputOption _outputOption = new("./network-policy.yaml");
  readonly KSailGenNativeClusterNetworkPolicyCommandHandler _handler = new();
  public KSailGenNativeClusterNetworkPolicyCommand() : base("network-policy", "Generate a 'networking.k8s.io/v1/NetworkPolicy' resource.")
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
