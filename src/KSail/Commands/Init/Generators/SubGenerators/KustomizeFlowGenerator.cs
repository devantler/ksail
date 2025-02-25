
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KustomizeFlowGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.Template.Kustomize.Hooks.Length == 0)
    {
      config.Spec.Template.Kustomize.Hooks = [""];
    }
    foreach (string currentHook in config.Spec.Template.Kustomize.Hooks)
    {
      foreach (string currentFlow in config.Spec.Template.Kustomize.Flows)
      {
        await GenerateKustomizeFlowHook(config, currentHook, currentFlow, cancellationToken).ConfigureAwait(false);
      }
    }
    if (config.Spec.DeploymentTool.Flux.PostBuildVariables)
    {
      foreach (string hook in config.Spec.Template.Kustomize.Hooks)
      {
        await GenerateKustomizeFlowHook(config, hook, "variables", cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateKustomizeFlowHook(KSailCluster config, string currentHook, string currentFlow, CancellationToken cancellationToken = default)
  {
    string outputPath = Path.Combine("k8s", currentHook, currentFlow);
    if (!Directory.Exists(outputPath))
      _ = Directory.CreateDirectory(outputPath);
    string outputDirectory = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{outputDirectory}'");
    string relativeRoot = string.Join("/", Enumerable.Repeat("..", Path.Combine(currentHook, currentFlow).Split('/').Length));
    KustomizeKustomization? kustomization;

    if (currentFlow == "variables")
    {
      string pathToNextFlow = currentHook == config.Spec.Template.Kustomize.Hooks.Last() ?
        "" :
        Path.Combine(
          relativeRoot,
          $"{config.Spec.Template.Kustomize.Hooks.ElementAt(
            Array.IndexOf([.. config.Spec.Template.Kustomize.Hooks], currentHook) + 1
          )}/{currentFlow}"
        );
      kustomization = new KustomizeKustomization
      {
        Resources = [
          "variables.yaml",
          "variables-sensitive.enc.yaml"
        ]
      };
      if (!string.IsNullOrEmpty(pathToNextFlow))
        kustomization.Resources = kustomization.Resources.Prepend(pathToNextFlow);
    }
    else
    {
      kustomization = currentHook == config.Spec.Template.Kustomize.Hooks.Last()
        ? new KustomizeKustomization
        {
          Resources = [],
        }
        : new KustomizeKustomization
        {
          Resources = [Path.Combine(relativeRoot, $"{config.Spec.Template.Kustomize.Hooks.ElementAt(Array.IndexOf([.. config.Spec.Template.Kustomize.Hooks], currentHook) + 1)}/{currentFlow}")],
        };
    }

    await _kustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
