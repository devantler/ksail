using KSail.Generator;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KSailClusterConfigGenerator
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string ksailConfigPath = Path.Combine(config.Spec.CLI.InitOptions.OutputDirectory, "ksail-config.yaml");
    if (File.Exists(ksailConfigPath))
    {
      Console.WriteLine($"✔ skipping '{ksailConfigPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{ksailConfigPath}'");
    await _ksailClusterGenerator.GenerateAsync(config, ksailConfigPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
