using System.Globalization;
using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class VariablesGenerator
{
  readonly ConfigMapGenerator _configMapGenerator = new();
  readonly SecretGenerator _secretGenerator = new();
  internal async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateClusterVariables(name, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionVariables(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalVariables(outputPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterVariables(string name, string outputPath, CancellationToken cancellationToken)
  {
    string clusterVariablesPath = Path.Combine(outputPath, "clusters", name, "variables");
    if (!Directory.Exists(clusterVariablesPath))
      _ = Directory.CreateDirectory(clusterVariablesPath);
    await GenerateVariablesConfigMap(clusterVariablesPath, "variables-cluster", cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(clusterVariablesPath, "variables-sensitive-cluster", cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionVariables(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionVariablesPath = Path.Combine(outputPath, "distributions", distribution.ToString().ToLower(CultureInfo.CurrentCulture), "variables");
    if (!Directory.Exists(distributionVariablesPath))
      _ = Directory.CreateDirectory(distributionVariablesPath);
    await GenerateVariablesConfigMap(distributionVariablesPath, "variables-distribution", cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(distributionVariablesPath, "variables-sensitive-distribution", cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalVariables(string outputPath, CancellationToken cancellationToken)
  {
    string globalVariablesPath = Path.Combine(outputPath, "variables");
    if (!Directory.Exists(globalVariablesPath))
      _ = Directory.CreateDirectory(globalVariablesPath);
    await GenerateVariablesConfigMap(globalVariablesPath, "variables-shared", cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(globalVariablesPath, "variables-sensitive-shared", cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateVariablesConfigMap(string outputPath, string name, CancellationToken cancellationToken)
  {
    string variablesConfigMapPath = Path.Combine(outputPath, "variables.yaml");
    if (File.Exists(variablesConfigMapPath))
    {
      Console.WriteLine($"✔ Skipping '{variablesConfigMapPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{variablesConfigMapPath}'");
    var configMap = new V1ConfigMap
    {
      ApiVersion = "v1",
      Kind = "ConfigMap",
      Metadata = new V1ObjectMeta
      {
        Name = name,
        NamespaceProperty = "flux-system"
      },
      Data = new Dictionary<string, string>()
    };
    await _configMapGenerator.GenerateAsync(configMap, variablesConfigMapPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateVariablesSensitiveSecret(string outputPath, string name, CancellationToken cancellationToken)
  {
    string variablesSensitiveSecretPath = Path.Combine(outputPath, "variables-sensitive.sops.yaml");
    if (File.Exists(variablesSensitiveSecretPath))
    {
      Console.WriteLine($"✔ Skipping '{variablesSensitiveSecretPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{variablesSensitiveSecretPath}'");
    var secret = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta
      {
        Name = name,
        NamespaceProperty = "flux-system"
      },
      StringData = new Dictionary<string, string>()
    };
    await _secretGenerator.GenerateAsync(secret, variablesSensitiveSecretPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
