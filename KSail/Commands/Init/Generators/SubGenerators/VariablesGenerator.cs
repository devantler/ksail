using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class VariablesGenerator
{
  readonly ConfigMapGenerator _configMapGenerator = new();
  readonly SecretGenerator _secretGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    foreach (string hook in config.Spec.InitOptions.KustomizeHooks)
    {
      string hookPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "k8s", hook, "variables");
      string name = hook.Replace("/", "-", StringComparison.Ordinal);
      await GenerateVariables(hookPath, name, cancellationToken).ConfigureAwait(false);
    }
  }

  async Task GenerateVariables(string outputPath, string name, CancellationToken cancellationToken = default)
  {
    if (!Directory.Exists(outputPath))
      _ = Directory.CreateDirectory(outputPath);
    string variableNameAffix = string.IsNullOrEmpty(name) ? "" : $"-{name}";
    await GenerateVariablesConfigMap(outputPath, $"variables{variableNameAffix}", cancellationToken).ConfigureAwait(false);
    await GenerateVariablesSensitiveSecret(outputPath, $"variables{variableNameAffix}-sensitive", cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateVariablesConfigMap(string outputPath, string name, CancellationToken cancellationToken = default)
  {
    string variablesConfigMapPath = Path.Combine(outputPath, "variables.yaml");
    if (File.Exists(variablesConfigMapPath))
    {
      Console.WriteLine($"✔ skipping '{variablesConfigMapPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{variablesConfigMapPath}'");
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

  async Task GenerateVariablesSensitiveSecret(string outputPath, string name, CancellationToken cancellationToken = default)
  {
    string variablesSensitiveSecretPath = Path.Combine(outputPath, "variables-sensitive.sops.yaml");
    if (File.Exists(variablesSensitiveSecretPath))
    {
      Console.WriteLine($"✔ skipping '{variablesSensitiveSecretPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{variablesSensitiveSecretPath}'");
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
