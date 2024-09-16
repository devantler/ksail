using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataClusterTrustBundleCommandHandler
{
  readonly ClusterTrustBundleGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1alpha1ClusterTrustBundle
    {
      ApiVersion = "certificates.k8s.io/v1alpha1",
      Kind = "ClusterTrustBundle",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1alpha1ClusterTrustBundleSpec()
      {
        SignerName = "<signer>",
        TrustBundle = "<trustBundle>"
      }
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
