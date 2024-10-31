
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KustomizeFlowGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    foreach (string currentHook in config.Spec.InitOptions.KustomizeHooks)
    {
      foreach (string currentFlow in config.Spec.InitOptions.KustomizeFlows)
      {
        await GenerateKustomizeFlowHook(config, currentHook, currentFlow, cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateKustomizeFlowHook(KSailCluster config, string currentHook, string currentFlow, CancellationToken cancellationToken)
  {
    string outputPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "k8s", currentHook, currentFlow);
    if (!Directory.Exists(outputPath))
      _ = Directory.CreateDirectory(outputPath);
    string outputDirectory = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ Skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{outputDirectory}'");
    string relativeRoot = string.Join("/", Enumerable.Repeat("..", Path.Combine(currentHook, currentFlow).Split('/').Length));
    var kustomization = new KustomizeKustomization
    {
      Resources = currentHook == config.Spec.InitOptions.KustomizeHooks.Last() ?
        config.Spec.InitOptions.HelmReleases && currentFlow == "infrastructure" ? ["cert-manager"] :
          config.Spec.InitOptions.HelmReleases && currentFlow == "apps" ? ["podinfo"] : [] :
        [Path.Combine(relativeRoot, $"{config.Spec.InitOptions.KustomizeHooks.ElementAt(Array.IndexOf(config.Spec.InitOptions.KustomizeHooks.ToArray(), currentHook) + 1)}/{currentFlow}")],
      Components = config.Spec.InitOptions.Components ?
        [
          Path.Combine(relativeRoot, "components/helm-release-crds-label"),
          Path.Combine(relativeRoot, "components/helm-release-remediation-label")
        ] : null
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
