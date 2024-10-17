
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KustomizeFlowGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  internal async Task GenerateAsync(TemplateGeneratorOptions options, CancellationToken cancellationToken)
  {
    foreach (string hook in options.KustomizeHooks)
    {
      foreach (string flow in options.KustomizeFlows)
      {
        await GenerateKustomizeFlowHook(options, hook, flow, cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateKustomizeFlowHook(TemplateGeneratorOptions options, string hook, string flow, CancellationToken cancellationToken)
  {
    string outputPath = Path.Combine(options.OutputPath, hook, flow);
    if (!Directory.Exists(outputPath))
      _ = Directory.CreateDirectory(outputPath);
    string relativePath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(relativePath))
    {
      Console.WriteLine($"✔ Skipping '{relativePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{relativePath}'");
    string relativeRoot = string.Join("/", Enumerable.Repeat("..", Path.Combine(hook, flow).Split('/').Length));
    var kustomization = new KustomizeKustomization
    {
      Resources = hook == options.KustomizeHooks.Last() ?
        options.IncludeHelmReleases && flow == "infrastructure" ? ["cert-manager"] :
          options.IncludeHelmReleases && flow == "apps" ? ["podinfo"] : [] :
        [Path.Combine(relativeRoot, $"{options.KustomizeHooks[Array.IndexOf(options.KustomizeHooks, hook) + 1]}/{flow}")],
      Components = options.IncludeComponents ?
        [
          Path.Combine(relativeRoot, "components/helm-release-crds-label"),
          Path.Combine(relativeRoot, "components/helm-release-remediation-label")
        ] : null
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, relativePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
