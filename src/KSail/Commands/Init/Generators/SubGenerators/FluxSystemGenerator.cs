using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Kustomization;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.Init.Generators.SubGenerators;

class FluxSystemGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  readonly FluxKustomizationGenerator _fluxKustomizationGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string outputDirectory = Path.Combine(config.Spec.Project.WorkingDirectory, "k8s", "clusters", config.Metadata.Name, "flux-system");
    if (!Directory.Exists(outputDirectory))
      _ = Directory.CreateDirectory(outputDirectory);
    await GenerateFluxSystemKustomization(config, outputDirectory, cancellationToken).ConfigureAwait(false);
    foreach (string flow in config.Spec.KustomizeTemplateOptions.Flows)
    {
      List<FluxDependsOn>? dependsOn = null;
      dependsOn = config.Spec.FluxDeploymentToolOptions.PostBuildVariables && config.Spec.KustomizeTemplateOptions.Flows.Length != 0
        ? config.Spec.KustomizeTemplateOptions.Flows.Last() == flow
          ? [new FluxDependsOn { Name = "variables" }]
          : [.. config.Spec.KustomizeTemplateOptions.Flows.Reverse().TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1)]
        : [.. config.Spec.KustomizeTemplateOptions.Flows.Reverse().TakeWhile(f => f != flow).Select(f => new FluxDependsOn { Name = f.Replace('/', '-') }).TakeLast(1)];

      await GenerateFluxSystemFluxKustomization(config, outputDirectory, flow, dependsOn, cancellationToken).ConfigureAwait(false);
    }
    if (config.Spec.FluxDeploymentToolOptions.PostBuildVariables)
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
      Resources = [.. config.Spec.KustomizeTemplateOptions.Flows.Select(flow => $"{flow.Replace('/', '-')}.yaml")]
    };
    if (config.Spec.FluxDeploymentToolOptions.PostBuildVariables)
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
      Metadata = new FluxNamespacedMetadata
      {
        Name = flow.Replace('/', '-'),
        Namespace = "flux-system"
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn = dependsOn,
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxKustomizationSpecSourceRefKind.OCIRepository,
          Name = "flux-system"
        },
        Path = config.Spec.KustomizeTemplateOptions.Hooks.Length == 0 ? flow : $"{config.Spec.KustomizeTemplateOptions.Hooks.First()}/{flow}",
        Prune = true,
        Wait = true,
        Decryption = config.Spec.Project.SecretManager == KSailSecretManager.SOPS ?
          new FluxKustomizationSpecDecryption
          {
            Provider = FluxKustomizationSpecDecryptionProvider.SOPS,
            SecretRef = new FluxSecretRef
            {
              Name = "sops-age"
              //TODO: Key = "sops.agekey"
            }
          } :
          null,
        PostBuild = config.Spec.FluxDeploymentToolOptions.PostBuildVariables ?
        new FluxKustomizationSpecPostBuild
        {
          SubstituteFrom = GetSubstituteFroms(config)
        } :
        null
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomization, outputDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  // TODO: Add support for Flux Kustomization post-build Variables
  IEnumerable<FluxKustomizationSpecPostBuildSubstituteFrom> GetSubstituteFroms(KSailCluster config)
  {
    var substituteList = new List<FluxKustomizationSpecPostBuildSubstituteFrom>
    {
      new() {
        Kind = FluxConfigRefKind.ConfigMap,
        Name = $"variables-cluster"
      },
      new() {
        Kind = FluxConfigRefKind.Secret,
        Name = $"variables-sensitive-cluster"
      }
    };
    foreach (string hook in config.Spec.KustomizeTemplateOptions.Hooks)
    {
      substituteList.Add(new FluxKustomizationSpecPostBuildSubstituteFrom
      {
        Kind = FluxConfigRefKind.ConfigMap,
        Name = $"variables-{hook}"
      });
      substituteList.Add(new FluxKustomizationSpecPostBuildSubstituteFrom
      {
        Kind = FluxConfigRefKind.Secret,
        Name = $"variables-sensitive-{hook}"
      });
    }
    return substituteList;
  }
}
