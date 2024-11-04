using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using k8s.Models;
using KSail.Models;
using Microsoft.IdentityModel.Tokens;

namespace KSail.Commands.Init.Generators.SubGenerators;

class FluxSystemGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  readonly FluxKustomizationGenerator _fluxKustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string outputDirectory = Path.Combine(config.Spec.InitOptions.OutputDirectory, "k8s", "clusters", config.Metadata.Name, "flux-system");
    if (!Directory.Exists(outputDirectory))
      _ = Directory.CreateDirectory(outputDirectory);
    await GenerateFluxSystemKustomization(config, outputDirectory, cancellationToken).ConfigureAwait(false);
    foreach (string flow in config.Spec.InitOptions.KustomizeFlows)
    {
      await GenerateFluxSystemFluxKustomization(config, outputDirectory, flow, cancellationToken).ConfigureAwait(false);
    }
    if (config.Spec.InitOptions.PostBuildVariables)
    {
      foreach (string hook in config.Spec.InitOptions.KustomizeHooks)
      {
        await GenerateFluxSystemFluxKustomization(config, outputDirectory, hook, cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateFluxSystemKustomization(KSailCluster config, string outputDirectory, CancellationToken cancellationToken = default)
  {
    outputDirectory = Path.Combine(outputDirectory, "kustomization.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ Skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{outputDirectory}'");
    var kustomization = new KustomizeKustomization
    {
      Resources = config.Spec.InitOptions.KustomizeFlows.Select(flow => $"{flow.Replace('/', '-')}.yaml").ToList(),
      Components = config.Spec.InitOptions.Components ?
        [
          "../../../components/flux-kustomization-post-build-variables-label",
          "../../../components/flux-kustomization-sops-label"
        ] :
        null,
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemFluxKustomization(KSailCluster config, string outputDirectory, string flow, CancellationToken cancellationToken = default)
  {
    outputDirectory = Path.Combine(outputDirectory, $"{flow.Replace('/', '-')}.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ Skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{outputDirectory}'");
    var fluxKustomization = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = flow.Replace('/', '-'),
        NamespaceProperty = "flux-system",
        Labels = config.Spec.Sops && config.Spec.InitOptions.Components ?
          new Dictionary<string, string>
          {
            { "kustomize.toolkit.fluxcd.io/sops", "enabled" }
          } :
          null
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn = config.Spec.InitOptions.PostBuildVariables && !config.Spec.InitOptions.KustomizeFlows.IsNullOrEmpty() && config.Spec.InitOptions.KustomizeFlows.First() == flow ?
          config.Spec.InitOptions.KustomizeHooks.Select(hook => new FluxDependsOn { Name = string.IsNullOrEmpty(hook) ? "variables" : $"variables-{hook}" }).ToList() :
          config.Spec.InitOptions.KustomizeFlows.TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1).ToList(),
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = config.Spec.InitOptions.KustomizeHooks.IsNullOrEmpty() ? flow : $"{config.Spec.InitOptions.KustomizeHooks.First()}/{flow}",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
