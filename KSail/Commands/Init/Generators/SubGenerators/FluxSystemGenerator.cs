using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using k8s.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class FluxSystemGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  readonly FluxKustomizationGenerator _fluxKustomizationGenerator = new();
  internal async Task GenerateAsync(TemplateGeneratorOptions options, CancellationToken cancellationToken)
  {
    string relativePath = Path.Combine(options.OutputPath, "clusters", options.ClusterName, "flux-system");
    if (!Directory.Exists(relativePath))
      _ = Directory.CreateDirectory(relativePath);
    await GenerateFluxSystemKustomization(options, relativePath, cancellationToken).ConfigureAwait(false);
    foreach (string flow in options.KustomizeFlows)
    {
      await GenerateFluxSystemFluxKustomization(options, relativePath, flow, cancellationToken).ConfigureAwait(false);
    }
    if (options.IncludeVariables)
    {
      foreach (string hook in options.KustomizeHooks)
      {
        await GenerateFluxSystemFluxKustomization(options, relativePath, hook, cancellationToken).ConfigureAwait(false);
      }
    }
  }

  async Task GenerateFluxSystemKustomization(TemplateGeneratorOptions options, string relativePath, CancellationToken cancellationToken)
  {
    relativePath = Path.Combine(relativePath, "kustomization.yaml");
    if (File.Exists(relativePath))
    {
      Console.WriteLine($"✔ Skipping '{relativePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{relativePath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources = options.KustomizeFlows.Select(flow => $"{flow.Replace('/', '-')}.yaml").ToList(),
      Components = options.IncludeComponents ?
        [
          "../../../components/flux-kustomization-post-build-variables-label",
          "../../../components/flux-kustomization-sops-label"
        ] :
        null,
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, relativePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemFluxKustomization(TemplateGeneratorOptions options, string relativePath, string flow, CancellationToken cancellationToken)
  {
    relativePath = Path.Combine(relativePath, $"{flow.Replace('/', '-')}.yaml");
    if (File.Exists(relativePath))
    {
      Console.WriteLine($"✔ Skipping '{relativePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{relativePath}'");
    var fluxKustomization = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = flow.Replace('/', '-'),
        NamespaceProperty = "flux-system",
        Labels = options.EnableSOPS && options.IncludeComponents ?
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
        DependsOn = options.IncludeVariables && options.KustomizeFlows[0] == flow ?
          options.KustomizeHooks.Select(hook => new FluxDependsOn { Name = string.IsNullOrEmpty(hook) ? "variables" : $"variables-{hook}" }).ToList() :
          options.KustomizeFlows.TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1).ToList(),
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = string.IsNullOrEmpty(options.KustomizeHooks[0]) ? flow : $"{options.KustomizeHooks[0]}/{flow}",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomization, relativePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
