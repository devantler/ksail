using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.KSail.Models;
using k8s.Models;

namespace KSail.Commands.Init.Generators;

class FluxSystemGenerator
{
  readonly FluxKustomizationGenerator _fluxKustomizationGenerator = new();
  internal async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemPath = Path.Combine(outputPath, "clusters", name, "flux-system");
    if (!Directory.Exists(fluxSystemPath))
    {
      _ = Directory.CreateDirectory(fluxSystemPath);
    }
    await GenerateFluxSystemKustomization(fluxSystemPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemFluxKustomizations(name, distribution, fluxSystemPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateFluxSystemKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(fluxSystemKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemKustomizationPath}'");
    await File.WriteAllTextAsync(fluxSystemKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemFluxKustomizations(string clusterName, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateFluxSystemVariablesFluxKustomization(clusterName, distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemInfrastructureFluxKustomization(outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemCustomResourcesFluxKustomization(outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemAppsFluxKustomization(outputPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemVariablesFluxKustomization(string clusterName, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemVariablesFluxKustomizationPath = Path.Combine(outputPath, "variables.yaml");
    if (File.Exists(fluxSystemVariablesFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemVariablesFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemVariablesFluxKustomizationPath}'");
    var fluxKustomizationVariablesCluster = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "variables-cluster",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
        {
          { "kustomize.toolkit.fluxcd.io", "enabled" }
        }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = $"clusters/{clusterName}/variables",
        Prune = true,
        Wait = true
      }
    };
    var fluxKustomizationVariablesDistribution = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "variables-distribution",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
        {
          { "kustomize.toolkit.fluxcd.io", "enabled" }
        }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = $"distributions/{distribution}/variables",
        Prune = true,
        Wait = true
      }
    };
    var fluxKustomizationVariablesGlobal = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "variables-global",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
          {
            { "kustomize.toolkit.fluxcd.io", "enabled" }
          }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = "variables",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationVariablesCluster, fluxSystemVariablesFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationVariablesDistribution, fluxSystemVariablesFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationVariablesGlobal, fluxSystemVariablesFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateFluxSystemInfrastructureFluxKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemInfrastructureFluxKustomizationPath = Path.Combine(outputPath, "infrastructure.yaml");
    if (File.Exists(fluxSystemInfrastructureFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemInfrastructureFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemInfrastructureFluxKustomizationPath}'");
    await File.WriteAllTextAsync(fluxSystemInfrastructureFluxKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateFluxSystemCustomResourcesFluxKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemCustomResourcesFluxKustomizationPath = Path.Combine(outputPath, "custom-resources.yaml");
    if (File.Exists(fluxSystemCustomResourcesFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemCustomResourcesFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemCustomResourcesFluxKustomizationPath}'");
    await File.WriteAllTextAsync(fluxSystemCustomResourcesFluxKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateFluxSystemAppsFluxKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemAppsFluxKustomizationPath = Path.Combine(outputPath, "apps.yaml");
    if (File.Exists(fluxSystemAppsFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemAppsFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemAppsFluxKustomizationPath}'");
    await File.WriteAllTextAsync(fluxSystemAppsFluxKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
