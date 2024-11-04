using Devantler.KubernetesGenerator.KSail;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KSailClusterConfigGenerator
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string ksailConfigPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "ksail-config.yaml");
    if (File.Exists(ksailConfigPath))
    {
      Console.WriteLine($"✔ Skipping '{ksailConfigPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{ksailConfigPath}'");
    await _ksailClusterGenerator.GenerateAsync(config, ksailConfigPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
