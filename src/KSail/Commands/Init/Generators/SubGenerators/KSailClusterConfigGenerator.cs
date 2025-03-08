using KSail.Generator;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KSailClusterConfigGenerator
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string outputPath = Path.Combine(config.Spec.Project.ConfigPath);
    bool overwrite = config.Spec.Generator.Overwrite;
    Console.WriteLine(File.Exists(outputPath) ? (overwrite ?
      $"✚ overwriting '{outputPath}'" :
      $"✔ skipping '{outputPath}', as it already exists.") :
      $"✚ generating '{outputPath}'");
    if (File.Exists(outputPath) && !overwrite)
    {
      return;
    }
    await _ksailClusterGenerator.GenerateAsync(config, outputPath, config.Spec.Generator.Overwrite, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
