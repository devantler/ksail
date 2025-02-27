using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class TemplateKustomizeGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string outputDirectory = Path.Combine(config.Spec.Project.KubernetesDirectoryPath, "clusters", config.Metadata.Name, "flux-system");
    if (!Directory.Exists(outputDirectory))
      _ = Directory.CreateDirectory(outputDirectory);
    await GenerateFluxSystemKustomization(outputDirectory, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemKustomization(string outputDirectory, CancellationToken cancellationToken = default)
  {
    outputDirectory = Path.Combine(outputDirectory, "kustomization.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{outputDirectory}'");
    var kustomization = new KustomizeKustomization()
    {
      Resources = []
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
