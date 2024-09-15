using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Commands.Init.Generators;

class VariablesGenerator
{
  internal static async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateClusterVariables(name, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionVariables(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalVariables(outputPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterVariables(string name, string outputPath, CancellationToken cancellationToken)
  {
    string clusterVariablesPath = Path.Combine(outputPath, "clusters", name, "variables");
    if (!Directory.Exists(clusterVariablesPath))
    {
      _ = Directory.CreateDirectory(clusterVariablesPath);
    }
    await GenerateVariablesConfigMap(clusterVariablesPath, cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(clusterVariablesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionVariables(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionVariablesPath = Path.Combine(outputPath, "distributions", distribution.ToString(), "variables");
    if (!Directory.Exists(distributionVariablesPath))
    {
      _ = Directory.CreateDirectory(distributionVariablesPath);
    }
    await GenerateVariablesConfigMap(distributionVariablesPath, cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(distributionVariablesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalVariables(string outputPath, CancellationToken cancellationToken)
  {
    string globalVariablesPath = Path.Combine(outputPath, "variables");
    if (!Directory.Exists(globalVariablesPath))
    {
      _ = Directory.CreateDirectory(globalVariablesPath);
    }
    await GenerateVariablesConfigMap(globalVariablesPath, cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(globalVariablesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateVariablesConfigMap(string outputPath, CancellationToken cancellationToken)
  {
    string variablesConfigMapPath = Path.Combine(outputPath, "variables.yaml");
    if (File.Exists(variablesConfigMapPath))
    {
      Console.WriteLine($"✔ Skipping '{variablesConfigMapPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{variablesConfigMapPath}'");
    await File.WriteAllTextAsync(variablesConfigMapPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateVariablesSensitiveSecret(string outputPath, CancellationToken cancellationToken)
  {
    string variablesSensitiveSecretPath = Path.Combine(outputPath, "variables-sensitive.sops.yaml");
    if (File.Exists(variablesSensitiveSecretPath))
    {
      Console.WriteLine($"✔ Skipping '{variablesSensitiveSecretPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{variablesSensitiveSecretPath}'");
    await File.WriteAllTextAsync(variablesSensitiveSecretPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
