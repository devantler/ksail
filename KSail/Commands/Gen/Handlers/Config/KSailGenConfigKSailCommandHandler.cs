
using Devantler.KubernetesGenerator.KSail;
using Devantler.KubernetesGenerator.KSail.Models;
using Devantler.KubernetesGenerator.KSail.Models.Registry;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigKSailCommandHandler
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var ksailCluster = new KSailCluster()
    {
      Metadata = new V1ObjectMeta()
      {
        Name = "default"
      },
      Spec = new KSailClusterSpec()
      {
        Distribution = KSailKubernetesDistribution.K3d,
        GitOpsTool = KSailGitOpsTool.Flux,
        Registries =
        [
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "ksail-registry",
            HostPort = 5050,
            IsGitOpsOCISource = true
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-docker.io",
            HostPort = 5001,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://registry-1.docker.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-registry.k8s.io",
            HostPort = 5002,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://registry.k8s.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-gcr.io",
            HostPort = 5003,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://gcr.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-ghcr.io",
            HostPort = 5004,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://ghcr.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-quay.io",
            HostPort = 5005,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://quay.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-mcr.microsoft.com",
            HostPort = 5006,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://mcr.microsoft.com"),
            }
          },
        ]
      }
    };
    await _ksailClusterGenerator.GenerateAsync(ksailCluster, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✚ Generating {outputPath}");
  }
}
