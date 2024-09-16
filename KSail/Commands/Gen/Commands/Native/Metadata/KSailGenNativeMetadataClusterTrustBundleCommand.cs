
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataClusterTrustBundleCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cluster-trust-bundle.yaml");
  readonly KSailGenNativeMetadataClusterTrustBundleCommandHandler _handler = new();
  public KSailGenNativeMetadataClusterTrustBundleCommand() : base("cluster-trust-bundle", "Generate a 'certificates.k8s.io/v1alpha1/ClusterTrustBundle' resource.")
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
