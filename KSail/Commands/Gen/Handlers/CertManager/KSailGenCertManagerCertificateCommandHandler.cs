using Devantler.KubernetesGenerator.CertManager;
using Devantler.KubernetesGenerator.CertManager.Models;
using Devantler.KubernetesGenerator.CertManager.Models.IssuerRef;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.CertManager;

class KSailGenCertManagerCertificateCommandHandler
{
  readonly CertManagerCertificateGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken)
  {
    var certificate = new CertManagerCertificate
    {
      Metadata = new V1ObjectMeta
      {
        Name = "cluster-issuer-certificate",
        NamespaceProperty = "traefik"
      },
      Spec = new CertManagerCertificateSpec
      {
        SecretName = "cluster-issuer-certificate-tls",
        DnsNames = [
          "k8s.local",
        ],
        IssuerRef = new CertManagerIssuerRef
        {
          Name = "selfsigned-cluster-issuer",
          Kind = "ClusterIssuer",
        }
      }
    };
    await _generator.GenerateAsync(certificate, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
