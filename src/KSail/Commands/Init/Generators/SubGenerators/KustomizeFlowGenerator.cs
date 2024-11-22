
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using KSail.Models;
using Microsoft.IdentityModel.Tokens;

namespace KSail.Commands.Init.Generators.SubGenerators;

class KustomizeFlowGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.CLI.InitOptions.KustomizeHooks.IsNullOrEmpty())
    {
      config.Spec.CLI.InitOptions.KustomizeHooks = [""];
    }
    foreach (string currentHook in config.Spec.CLI.InitOptions.KustomizeHooks)
    {
      foreach (string currentFlow in config.Spec.CLI.InitOptions.KustomizeFlows)
      {
        await GenerateKustomizeFlowHook(config, currentHook, currentFlow, cancellationToken).ConfigureAwait(false);
      }
    }
    if (config.Spec.CLI.InitOptions.PostBuildVariables)
    {
      foreach (string hook in config.Spec.CLI.InitOptions.KustomizeHooks)
      {
        await GenerateKustomizeFlowHook(config, hook, "variables", cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateKustomizeFlowHook(KSailCluster config, string currentHook, string currentFlow, CancellationToken cancellationToken = default)
  {
    string outputPath = Path.Combine(config.Spec.CLI.InitOptions.OutputDirectory, "k8s", currentHook, currentFlow);
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
      string pathToNextFlow = currentHook == config.Spec.CLI.InitOptions.KustomizeHooks.Last() ?
        "" :
        Path.Combine(
          relativeRoot,
          $"{config.Spec.CLI.InitOptions.KustomizeHooks.ElementAt(
            Array.IndexOf(config.Spec.CLI.InitOptions.KustomizeHooks.ToArray(), currentHook) + 1
          )}/{currentFlow}"
        );
      kustomization = new KustomizeKustomization
      {
        Resources = [
          Path.Combine($"variables.yaml"),
          Path.Combine($"variables-sensitive.sops.yaml")
        ],
        Components = config.Spec.CLI.InitOptions.Components ?
        [
          Path.Combine(relativeRoot, "components/helm-release-crds-label"),
          Path.Combine(relativeRoot, "components/helm-release-remediation-label")
        ] : null
      };
      if (!string.IsNullOrEmpty(pathToNextFlow))
        kustomization.Resources = kustomization.Resources.Prepend(pathToNextFlow);
    }
    else
    {
      kustomization = currentHook == config.Spec.CLI.InitOptions.KustomizeHooks.Last()
        ? new KustomizeKustomization
        {
          Resources = config.Spec.CLI.InitOptions.HelmReleases && currentFlow == "infrastructure/controllers" ? ["cert-manager", "traefik"] :
                config.Spec.CLI.InitOptions.HelmReleases && currentFlow == "infrastructure" ? ["certificates", "cluster-issuers"] :
                  config.Spec.CLI.InitOptions.HelmReleases && currentFlow == "apps" ? ["podinfo"] : [],
          Components = config.Spec.CLI.InitOptions.Components ?
              [
                Path.Combine(relativeRoot, "components/helm-release-crds-label"),
          Path.Combine(relativeRoot, "components/helm-release-remediation-label")
              ] : null
        }
        : new KustomizeKustomization
        {
          Resources = [Path.Combine(relativeRoot, $"{config.Spec.CLI.InitOptions.KustomizeHooks.ElementAt(Array.IndexOf(config.Spec.CLI.InitOptions.KustomizeHooks.ToArray(), currentHook) + 1)}/{currentFlow}")],
          Components = config.Spec.CLI.InitOptions.Components ?
              [
                Path.Combine(relativeRoot, "components/helm-release-crds-label"),
          Path.Combine(relativeRoot, "components/helm-release-remediation-label")
              ] : null
        };
    }

    await _kustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}