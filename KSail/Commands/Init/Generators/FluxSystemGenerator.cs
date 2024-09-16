using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.KSail.Models;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using k8s.Models;

namespace KSail.Commands.Init.Generators;

class FluxSystemGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
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

  async Task GenerateFluxSystemKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(fluxSystemKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "apps.yaml",
        "custom-resources.yaml",
        "infrastructure.yaml",
        "variables.yaml"
      ],
      Components = [
        "../../../components/flux-kustomization-post-build-variables-label",
        "../../../components/flux-kustomization-sops-label"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, fluxSystemKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemFluxKustomizations(string clusterName, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateFluxSystemVariablesFluxKustomization(clusterName, distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemInfrastructureFluxKustomization(clusterName, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemCustomResourcesFluxKustomization(clusterName, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateFluxSystemAppsFluxKustomization(clusterName, outputPath, cancellationToken).ConfigureAwait(false);
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
          { "kustomize.toolkit.fluxcd.io/sops", "enabled" }
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
          { "kustomize.toolkit.fluxcd.io/sops", "enabled" }
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
        Path = $"distributions/{distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}/variables",
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
            { "kustomize.toolkit.fluxcd.io/sops", "enabled" }
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

  async Task GenerateFluxSystemInfrastructureFluxKustomization(string clusterName, string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemInfrastructureFluxKustomizationPath = Path.Combine(outputPath, "infrastructure.yaml");
    if (File.Exists(fluxSystemInfrastructureFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemInfrastructureFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemInfrastructureFluxKustomizationPath}'");
    var fluxKustomizationInfrastructure = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "infrastructure",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
        {
          { "kustomize.toolkit.fluxcd.io/sops", "enabled" },
          { "kustomize.toolkit.fluxcd.io/post-build-variables", "enabled" }
        }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn =
        [
          new FluxDependsOn
          {
            Name = "variables-cluster"
          },
          new FluxDependsOn
          {
            Name = "variables-distribution"
          },
          new FluxDependsOn
          {
            Name = "variables-global"
          }
        ],
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = $"clusters/{clusterName}/infrastructure",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationInfrastructure, fluxSystemInfrastructureFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemCustomResourcesFluxKustomization(string clusterName, string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemCustomResourcesFluxKustomizationPath = Path.Combine(outputPath, "custom-resources.yaml");
    if (File.Exists(fluxSystemCustomResourcesFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemCustomResourcesFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemCustomResourcesFluxKustomizationPath}'");
    var fluxKustomizationCustomResources = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "custom-resources",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
        {
          { "kustomize.toolkit.fluxcd.io/sops", "enabled" },
          { "kustomize.toolkit.fluxcd.io/post-build-variables", "enabled" }
        }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn =
        [
          new FluxDependsOn
          {
            Name = "infrastructure"
          }
        ],
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = $"clusters/{clusterName}/custom-resources",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationCustomResources, fluxSystemCustomResourcesFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxSystemAppsFluxKustomization(string clusterName, string outputPath, CancellationToken cancellationToken)
  {
    string fluxSystemAppsFluxKustomizationPath = Path.Combine(outputPath, "apps.yaml");
    if (File.Exists(fluxSystemAppsFluxKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxSystemAppsFluxKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxSystemAppsFluxKustomizationPath}'");
    var fluxKustomizationApps = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "apps",
        NamespaceProperty = "flux-system",
        Labels = new Dictionary<string, string>
        {
          { "kustomize.toolkit.fluxcd.io/sops", "enabled" },
          { "kustomize.toolkit.fluxcd.io/post-build-variables", "enabled" }
        }
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn =
        [
          new FluxDependsOn
          {
            Name = "custom-resources"
          }
        ],
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSource.OCIRepository,
          Name = "flux-system"
        },
        Path = $"clusters/{clusterName}/apps",
        Prune = true,
        Wait = true
      }
    };
    await _fluxKustomizationGenerator.GenerateAsync(fluxKustomizationApps, fluxSystemAppsFluxKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
