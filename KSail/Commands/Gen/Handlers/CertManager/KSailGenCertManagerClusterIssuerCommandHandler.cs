using Devantler.KubernetesGenerator.CertManager;
using Devantler.KubernetesGenerator.CertManager.Models;
using Devantler.KubernetesGenerator.KSail.Models;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.CertManager;

class KSailGenCertManagerClusterIssuerCommandHandler
{
  readonly KSailCluster _config;
  readonly CertManagerClusterIssuerGenerator _generator = new();

  internal KSailGenCertManagerClusterIssuerCommandHandler(KSailCluster config) => _config = config;

  internal async Task HandleAsync(CancellationToken cancellationToken = default)
  {
    var clusterIssuer = new CertManagerClusterIssuer
    {
      Metadata = new V1ObjectMeta
      {
        Name = "selfsigned-cluster-issuer",
        NamespaceProperty = "cert-manager"
      },
      Spec = new CertManagerClusterIssuerSpec
      {
        SelfSigned = new object()

      }
    };
    await _generator.GenerateAsync(clusterIssuer, _config.Spec?.ManifestsDirectory!, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
