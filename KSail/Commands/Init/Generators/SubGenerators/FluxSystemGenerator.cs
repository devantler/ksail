using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.SecretRef;
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
      List<FluxDependsOn> dependsOn = [];
      dependsOn = config.Spec.InitOptions.PostBuildVariables && !config.Spec.InitOptions.KustomizeFlows.IsNullOrEmpty()
        ? config.Spec.InitOptions.KustomizeFlows.Last() == flow
          ? ([new FluxDependsOn { Name = "variables" }])
          : config.Spec.InitOptions.KustomizeFlows.Reverse().TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1).ToList()
        : config.Spec.InitOptions.KustomizeFlows.Reverse().TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1).ToList();

      await GenerateFluxSystemFluxKustomization(config, outputDirectory, flow, dependsOn, cancellationToken).ConfigureAwait(false);
    }
    if (config.Spec.InitOptions.PostBuildVariables)
    {
      await GenerateFluxSystemFluxKustomization(config, outputDirectory, "variables", [], cancellationToken).ConfigureAwait(false);
    }
  }

  async Task GenerateFluxSystemKustomization(KSailCluster config, string outputDirectory, CancellationToken cancellationToken = default)
  {
    outputDirectory = Path.Combine(outputDirectory, "kustomization.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{outputDirectory}'");
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
    if (config.Spec.InitOptions.PostBuildVariables)
    {
      kustomization.Resources = kustomization.Resources.Append("variables.yaml");
    }
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemFluxKustomization(KSailCluster config, string outputDirectory, string flow, IEnumerable<FluxDependsOn> dependsOn, CancellationToken cancellationToken = default)
  {
    outputDirectory = Path.Combine(outputDirectory, $"{flow.Replace('/', '-')}.yaml");
    if (File.Exists(outputDirectory))
    {
      Console.WriteLine($"✔ skipping '{outputDirectory}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{outputDirectory}'");
    var fluxKustomization = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = flow.Replace('/', '-'),
        NamespaceProperty = "flux-system",
        Labels = config.Spec.Sops && config.Spec.InitOptions.PostBuildVariables && config.Spec.InitOptions.Components && !flow.Equals("variables") ?
          new Dictionary<string, string>
          {
            { "sops", "enabled" },
            { "post-build-variables", "enabled" }
          } : config.Spec.InitOptions.PostBuildVariables && config.Spec.InitOptions.Components && !flow.Equals("variables") ?
          new Dictionary<string, string>
          {
            { "post-build-variables", "enabled" }
          } : config.Spec.Sops && config.Spec.InitOptions.Components ?
          new Dictionary<string, string>
          {
            { "sops", "enabled" }
          } :
          null
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn = dependsOn,
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = config.Spec.InitOptions.KustomizeHooks.IsNullOrEmpty() ? flow : $"{config.Spec.InitOptions.KustomizeHooks.First()}/{flow}",
        Prune = true,
        Wait = true,
        Decryption = config.Spec.Sops && !config.Spec.InitOptions.Components ?
          new FluxKustomizationSpecDecryption
          {
            Provider = FluxKustomizationSpecDecryptionProvider.SOPS,
            SecretRef = new FluxSecretRef
            {
              Name = "sops-age",
              Key = "sops.agekey"
            }
          } :
          null,
        PostBuild = config.Spec.InitOptions.PostBuildVariables && !config.Spec.InitOptions.Components ?
          new FluxKustomizationSpecPostBuild
          {
            SubstituteFrom = GetSubstituteFroms(config)
          } :
          null
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  IEnumerable<FluxKustomizationSpecPostBuildSubstituteFrom> GetSubstituteFroms(KSailCluster config)
  {
    var substituteList = new List<FluxKustomizationSpecPostBuildSubstituteFrom>
    {
      new() {
        Kind = FluxKustomizationSpecPostBuildSubstituteFromKind.ConfigMap,
        Name = $"variables-cluster"
      },
      new() {
        Kind = FluxKustomizationSpecPostBuildSubstituteFromKind.Secret,
        Name = $"variables-sensitive-cluster"
      }
    };
    foreach (string hook in config.Spec.InitOptions.KustomizeHooks)
    {
      substituteList.Add(new FluxKustomizationSpecPostBuildSubstituteFrom
      {
        Kind = FluxKustomizationSpecPostBuildSubstituteFromKind.ConfigMap,
        Name = $"variables-{hook}"
      });
      substituteList.Add(new FluxKustomizationSpecPostBuildSubstituteFrom
      {
        Kind = FluxKustomizationSpecPostBuildSubstituteFromKind.Secret,
        Name = $"variables-sensitive-{hook}"
      });
    }
    return substituteList;
  }
}
