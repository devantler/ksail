using System.Globalization;
using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class AppsGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  readonly NamespaceGenerator _namespaceGenerator = new();
  readonly FluxHelmReleaseGenerator _helmReleaseGenerator = new();
  readonly FluxHelmRepositoryGenerator _helmRepositoryGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    await GenerateClusterApps(name, distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionApps(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalApps(outputPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterApps(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string clusterAppsPath = Path.Combine(outputPath, "clusters", name, "apps");
    if (!Directory.Exists(clusterAppsPath))
      _ = Directory.CreateDirectory(clusterAppsPath);
    await GenerateClusterAppsKustomization(distribution, clusterAppsPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterAppsKustomization(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string clusterAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(clusterAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterAppsKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        $"../../../distributions/{distribution.ToString().ToLower(CultureInfo.CurrentCulture)}/apps"
      ],
      Components = [
        "../../../components/helm-release-crds-label",
        "../../../components/helm-release-remediation-label"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, clusterAppsKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionApps(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionAppsPath = Path.Combine(outputPath, "distributions", distribution.ToString().ToLower(CultureInfo.CurrentCulture), "apps");
    if (!Directory.Exists(distributionAppsPath))
      _ = Directory.CreateDirectory(distributionAppsPath);
    await GenerateDistributionAppsKustomization(distributionAppsPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionAppsKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionAppsKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "../../../apps"
      ],
      Components = [
        "../../../components/helm-release-crds-label",
        "../../../components/helm-release-remediation-label"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, distributionAppsKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalApps(string outputPath, CancellationToken cancellationToken)
  {
    string globalAppsPath = Path.Combine(outputPath, "apps");
    if (!Directory.Exists(globalAppsPath))
      _ = Directory.CreateDirectory(globalAppsPath);
    await GenerateGlobalAppsKustomization(globalAppsPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodinfo(globalAppsPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalAppsKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalAppsKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "podinfo"
      ],
      Components = [
        "../components/helm-release-crds-label",
        "../components/helm-release-remediation-label"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, globalAppsKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodinfo(string outputPath, CancellationToken cancellationToken)
  {
    string podinfoPath = Path.Combine(outputPath, "podinfo");
    if (!Directory.Exists(podinfoPath))
      _ = Directory.CreateDirectory(podinfoPath);

    await GeneratePodInfoKustomization(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoNamespace(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoHelmRelease(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoHelmRepository(podinfoPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodInfoKustomization(string podinfoPath, CancellationToken cancellationToken)
  {
    string podinfoKustomizationPath = Path.Combine(podinfoPath, "kustomization.yaml");
    if (File.Exists(podinfoKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "namespace.yaml",
        "helm-release.yaml",
        "helm-repository.yaml"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, podinfoKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodInfoNamespace(string podinfoPath, CancellationToken cancellationToken)
  {
    string podinfoNamespacePath = Path.Combine(podinfoPath, "namespace.yaml");
    if (File.Exists(podinfoNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoNamespacePath}'");
    var @namespace = new V1Namespace
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta
      {
        Name = "podinfo"
      }
    };
    await _namespaceGenerator.GenerateAsync(@namespace, podinfoNamespacePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodInfoHelmRelease(string podinfoPath, CancellationToken cancellationToken)
  {
    string podinfoHelmReleasePath = Path.Combine(podinfoPath, "helm-release.yaml");
    if (File.Exists(podinfoHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoHelmReleasePath}'");
    var helmRelease = new FluxHelmRelease
    {
      Metadata = new V1ObjectMeta
      {
        Name = "podinfo",
        NamespaceProperty = "podinfo"
      },
      Spec = new FluxHelmReleaseSpec
      {
        Interval = "10m",
        Chart = new FluxHelmReleaseSpecChart
        {
          Spec = new FluxHelmReleaseSpecChartSpec
          {
            Chart = "podinfo",
            SourceRef = new FluxSourceRef
            {
              Kind = FluxSource.HelmRepository,
              Name = "podinfo"
            }
          }
        }
      }
    };
    await _helmReleaseGenerator.GenerateAsync(helmRelease, podinfoHelmReleasePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodInfoHelmRepository(string podinfoPath, CancellationToken cancellationToken)
  {
    string podinfoHelmRepositoryPath = Path.Combine(podinfoPath, "helm-repository.yaml");
    if (File.Exists(podinfoHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoHelmRepositoryPath}'");
    var helmRepository = new FluxHelmRepository
    {
      Metadata = new V1ObjectMeta
      {
        Name = "podinfo",
        NamespaceProperty = "podinfo"
      },
      Spec = new FluxHelmRepositorySpec
      {
        Interval = "10m",
        Url = new Uri("oci://ghcr.io/stefanprodan/charts"),
        Type = FluxHelmRepositorySpecType.OCI
      }
    };
    await _helmRepositoryGenerator.GenerateAsync(helmRepository, podinfoHelmRepositoryPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

}
