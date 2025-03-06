using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Docker.DotNet.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class TemplateKustomizeGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string outputDirectory = Path.IsPathRooted(config.Spec.Project.KustomizationPath)
      ? config.Spec.Project.KustomizationPath
      : Path.Combine(config.Spec.Project.KustomizationPath);

    if (!Directory.Exists(outputDirectory))
      _ = Directory.CreateDirectory(outputDirectory);
    await GenerateKustomization(config, outputDirectory, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateKustomization(KSailCluster config, string path, CancellationToken cancellationToken = default)
  {
    path = Path.Combine(path, "kustomization.yaml");
    if (File.Exists(path) && !config.Spec.Generator.Overwrite)
    {
      Console.WriteLine($"✔ skipping '{path}', as it already exists.");
      return;
    }
    else if (File.Exists(path) && config.Spec.Generator.Overwrite)
    {
      Console.WriteLine($"✚ overwriting '{path}'");
    }
    else
    {
      Console.WriteLine($"✚ generating '{path}'");
    }
    var kustomization = new KustomizeKustomization()
    {
      Resources = []
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, path, config.Spec.Generator.Overwrite, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
