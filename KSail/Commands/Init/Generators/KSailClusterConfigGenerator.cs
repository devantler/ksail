using Devantler.KubernetesGenerator.KSail;
using Devantler.KubernetesGenerator.KSail.Models;
using Devantler.KubernetesGenerator.KSail.Models.Registry;
using k8s.Models;

namespace KSail.Commands.Init.Generators;

class KSailClusterConfigGenerator
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, KSailGitOpsTool gitOpsTool, string outputPath, CancellationToken cancellationToken)
  {
    var ksailCluster = new KSailCluster()
    {
      Metadata = new V1ObjectMeta()
      {
        Name = name
      },
      Spec = new KSailClusterSpec()
      {
        Distribution = distribution,
        GitOpsTool = gitOpsTool,
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
            HostPort = 5000,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://registry-1.docker.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-ghcr.io",
            HostPort = 5001,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://ghcr.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-gcr.io",
            HostPort = 5002,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://gcr.io"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-mcr.microsoft.com",
            HostPort = 5003,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://mcr.microsoft.com"),
            }
          },
          new KSailRegistry()
          {
            Provider = KSailRegistryProvider.Docker,
            Name = "proxy-registry.k8s.io",
            HostPort = 5004,
            Proxy = new KSailRegistryProxy()
            {
               Url = new Uri("https://registry.k8s.io"),
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
        ]
      }
    };
    string ksailConfigPath = Path.Combine(outputPath, "ksail-config.yaml");
    if (File.Exists(ksailConfigPath))
    {
      Console.WriteLine($"✔ Skipping '{ksailConfigPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{ksailConfigPath}'");
    await _ksailClusterGenerator.GenerateAsync(ksailCluster, ksailConfigPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
