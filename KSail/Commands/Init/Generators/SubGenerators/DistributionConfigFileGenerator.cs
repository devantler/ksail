using Devantler.KubernetesGenerator.K3d;
using Devantler.KubernetesGenerator.K3d.Models;
using Devantler.KubernetesGenerator.KSail.Models;
using k8s.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class DistributionConfigFileGenerator
{
  readonly K3dConfigGenerator _k3dConfigKubernetesGenerator = new();

  internal async Task GenerateAsync(string clusterName, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionConfigPath = distribution switch
    {
      KSailKubernetesDistribution.K3d => Path.Combine(outputPath, "k3d-config.yaml"),
      _ => throw new NotSupportedException($"Distribution '{distribution}' is not supported.")
    };
    if (File.Exists(distributionConfigPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionConfigPath}', as it already exists.");
      return;
    }
    switch (distribution)
    {
      case KSailKubernetesDistribution.K3d:
        await GenerateK3DConfigFile(clusterName, distributionConfigPath, cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"Distribution '{distribution}' is not supported.");
    }
  }

  async Task GenerateK3DConfigFile(string clusterName, string outputPath, CancellationToken cancellationToken)
  {
    Console.WriteLine($"✚ Generating '{outputPath}'");
    var k3dConfig = new K3dConfig
    {
      Metadata = new V1ObjectMeta
      {
        Name = clusterName
      },
      Options = new K3dConfigOptions
      {
        K3s = new K3dConfigOptionsK3s
        {
          ExtraArgs = [
            new K3dConfigOptionsK3sExtraArg
            {
              Arg = "--disable=traefik",
              NodeFilters = [
                "server:*"
              ]
            }
          ]
        }
      },
      Registries = new K3dConfigRegistries
      {
        Config = """
          mirrors:
            "docker.io":
              endpoint:
                - http://host.k3d.internal:5001
            "registry.k8s.io":
              endpoint:
                - http://host.k3d.internal:5002
            "gcr.io":
              endpoint:
                - http://host.k3d.internal:5003
            "ghcr.io":
              endpoint:
                - http://host.k3d.internal:5004
            "quay.io":
              endpoint:
                - http://host.k3d.internal:5005
            "mcr.microsoft.com":
              endpoint:
                - http://host.k3d.internal:5006
        """
      }
    };

    await _k3dConfigKubernetesGenerator.GenerateAsync(k3dConfig, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
