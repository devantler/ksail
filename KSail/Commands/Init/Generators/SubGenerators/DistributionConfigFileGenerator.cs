using System.Text;
using Devantler.KubernetesGenerator.K3d;
using Devantler.KubernetesGenerator.K3d.Models;
using Devantler.KubernetesGenerator.K3d.Models.Options;
using Devantler.KubernetesGenerator.K3d.Models.Options.K3s;
using Devantler.KubernetesGenerator.K3d.Models.Registries;
using Devantler.KubernetesGenerator.Kind;
using Devantler.KubernetesGenerator.Kind.Models;
using k8s.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class DistributionConfigFileGenerator
{
  readonly K3dConfigGenerator _k3dConfigKubernetesGenerator = new();
  readonly KindConfigGenerator _kindConfigKubernetesGenerator = new();

  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    string distributionConfigPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, $"{config.Spec.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-config.yaml");
    if (File.Exists(distributionConfigPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionConfigPath}', as it already exists.");
      return;
    }
    switch (config.Spec.Distribution)
    {
      case KSailKubernetesDistribution.Kind:
        await GenerateKindConfigFile(config, distributionConfigPath, cancellationToken).ConfigureAwait(false);
        break;
      case KSailKubernetesDistribution.K3d:
        await GenerateK3DConfigFile(config, distributionConfigPath, cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"Distribution '{config.Spec.Distribution}' is not supported.");
    }
  }

  async Task GenerateKindConfigFile(KSailCluster config, string outputPath, CancellationToken cancellationToken)
  {
    Console.WriteLine($"✚ Generating '{outputPath}'");
    var kindConfig = new KindConfig
    {
      Name = config.Metadata.Name
    };

    await _kindConfigKubernetesGenerator.GenerateAsync(kindConfig, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateK3DConfigFile(KSailCluster config, string outputPath, CancellationToken cancellationToken)
  {
    Console.WriteLine($"✚ Generating '{outputPath}'");
    var mirrors = new StringBuilder();
    mirrors = mirrors.AppendLine("mirrors:");
    foreach (var registry in config.Spec.Registries.Where(x => !x.IsGitOpsOCISource))
    {
      string mirror = $"""
      "{registry.Name}":
        endpoint:
          - {registry.Proxy}
      """;
      mirrors = mirrors.AppendLine("    " + mirror);
    }
    var k3dConfig = new K3dConfig
    {
      Metadata = new V1ObjectMeta
      {
        Name = config.Metadata.Name
      },
      Options = new K3dOptions
      {
        K3s = new K3dOptionsK3s
        {
          ExtraArgs = [
            new K3dOptionsK3sExtraArg
            {
              Arg = "--disable=traefik",
              NodeFilters = [
                "server:*"
              ]
            }
          ]
        }
      },
      Registries = new K3dRegistries
      {
        Config = $"""
          {mirrors}
        """
      }
    };

    await _k3dConfigKubernetesGenerator.GenerateAsync(k3dConfig, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}