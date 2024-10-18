using System.Globalization;
using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class InfrastructureGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizeKustomizationGenerator = new();
  readonly NamespaceGenerator _namespaceGenerator = new();
  readonly FluxHelmReleaseGenerator _helmReleaseGenerator = new();
  readonly FluxHelmRepositoryGenerator _helmRepositoryGenerator = new();
  internal async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateClusterInfrastructure(name, distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionInfrastructure(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalInfrastructure(outputPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterInfrastructure(string name, KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string clusterInfrastructurePath = Path.Combine(outputPath, "clusters", name, "infrastructure");
    if (!Directory.Exists(clusterInfrastructurePath))
      _ = Directory.CreateDirectory(clusterInfrastructurePath);
    await GenerateClusterInfrastructureKustomization(distribution, clusterInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterInfrastructureKustomization(KSailKubernetesDistribution distribution, string clusterInfrastructurePath, CancellationToken cancellationToken)
  {
    string clusterInfrastructureKustomizationPath = Path.Combine(clusterInfrastructurePath, "kustomization.yaml");
    if (File.Exists(clusterInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterInfrastructureKustomizationPath}'");
    var kustomization = new KustomizeKustomization()
    {
      Resources = [
        $"../../../distributions/{distribution.ToString().ToLower(CultureInfo.CurrentCulture)}/infrastructure"
      ],
      Components = [
        "../../../components/helm-release-crds-label",
        "../../../components/helm-release-remediation-label"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, clusterInfrastructureKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionInfrastructure(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionInfrastructurePath = Path.Combine(outputPath, "distributions", distribution.ToString().ToLower(CultureInfo.CurrentCulture), "infrastructure");
    if (!Directory.Exists(distributionInfrastructurePath))
      _ = Directory.CreateDirectory(distributionInfrastructurePath);
    await GenerateDistributionInfrastructureKustomization(distributionInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionInfrastructureKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionInfrastructureKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionInfrastructureKustomizationPath}'");
    var kustomization = new KustomizeKustomization()
    {
      Resources = [
        "../../../infrastructure"
      ],
      Components = [
        "../../../components/helm-release-crds-label",
        "../../../components/helm-release-remediation-label"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, distributionInfrastructureKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalInfrastructure(string outputPath, CancellationToken cancellationToken)
  {
    string globalInfrastructurePath = Path.Combine(outputPath, "infrastructure");
    if (!Directory.Exists(globalInfrastructurePath))
      _ = Directory.CreateDirectory(globalInfrastructurePath);
    await GenerateGlobalInfrastructureKustomization(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
    await GenerateCertManager(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
    await GenerateTraefik(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalInfrastructureKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalInfrastructureKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalInfrastructureKustomizationPath}'");
    var kustomization = new KustomizeKustomization()
    {
      Resources = [
        "cert-manager",
        "traefik"
      ],
      Components = [
        "../components/helm-release-crds-label",
        "../components/helm-release-remediation-label"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(kustomization, globalInfrastructureKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateCertManager(string outputPath, CancellationToken cancellationToken)
  {
    string certManagerPath = Path.Combine(outputPath, "cert-manager");
    if (!Directory.Exists(certManagerPath))
      _ = Directory.CreateDirectory(certManagerPath);

    string certManagerKustomizationPath = Path.Combine(certManagerPath, "kustomization.yaml");
    if (File.Exists(certManagerKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerKustomizationPath}'");
    var certManagerKustomization = new KustomizeKustomization()
    {
      Resources = [
        "namespace.yaml",
        "helm-release.yaml",
        "helm-repository.yaml"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(certManagerKustomization, certManagerKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string certManagerNamespacePath = Path.Combine(certManagerPath, "namespace.yaml");
    if (File.Exists(certManagerNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerNamespacePath}'");
    var certManagerNamespace = new V1Namespace
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta
      {
        Name = "cert-manager"
      }
    };
    await _namespaceGenerator.GenerateAsync(certManagerNamespace, certManagerNamespacePath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string certManagerHelmReleasePath = Path.Combine(certManagerPath, "helm-release.yaml");
    if (File.Exists(certManagerHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerHelmReleasePath}'");
    var certManagerHelmRelease = new FluxHelmRelease()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "cert-manager",
        NamespaceProperty = "cert-manager"
      },
      Spec = new FluxHelmReleaseSpec
      {
        Interval = "10m",
        Chart = new FluxHelmReleaseSpecChart
        {
          Spec = new FluxHelmReleaseSpecChartSpec
          {
            Chart = "cert-manager",
            Version = "v1.15.3",
            SourceRef = new FluxSourceRef
            {
              Kind = FluxSource.HelmRepository,
              Name = "cert-manager"
            }
          }
        },
        Values = new Dictionary<string, object>
        {
          ["crds"] = new Dictionary<string, object>
          {
            ["enabled"] = true
          }
        }
      }
    };
    await _helmReleaseGenerator.GenerateAsync(certManagerHelmRelease, certManagerHelmReleasePath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string certManagerHelmRepositoryPath = Path.Combine(certManagerPath, "helm-repository.yaml");
    if (File.Exists(certManagerHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerHelmRepositoryPath}'");
    var certManagerHelmRepository = new FluxHelmRepository()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "cert-manager",
        NamespaceProperty = "cert-manager"
      },
      Spec = new FluxHelmRepositorySpec
      {
        Url = new Uri("https://charts.jetstack.io")
      }
    };
    await _helmRepositoryGenerator.GenerateAsync(certManagerHelmRepository, certManagerHelmRepositoryPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateTraefik(string outputPath, CancellationToken cancellationToken)
  {
    string traefikPath = Path.Combine(outputPath, "traefik");
    if (!Directory.Exists(traefikPath))
      _ = Directory.CreateDirectory(traefikPath);

    string traefikKustomizationPath = Path.Combine(traefikPath, "kustomization.yaml");
    if (File.Exists(traefikKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{traefikKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikKustomizationPath}'");
    var traefikKustomization = new KustomizeKustomization()
    {
      Resources = [
        "namespace.yaml",
        "helm-release.yaml",
        "helm-repository.yaml"
      ]
    };
    await _kustomizeKustomizationGenerator.GenerateAsync(traefikKustomization, traefikKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string traefikNamespacePath = Path.Combine(traefikPath, "namespace.yaml");
    if (File.Exists(traefikNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{traefikNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikNamespacePath}'");
    var traefikNamespace = new V1Namespace
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta
      {
        Name = "traefik"
      }
    };
    await _namespaceGenerator.GenerateAsync(traefikNamespace, traefikNamespacePath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string traefikHelmReleasePath = Path.Combine(traefikPath, "helm-release.yaml");
    if (File.Exists(traefikHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{traefikHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikHelmReleasePath}'");
    var traefikHelmRelease = new FluxHelmRelease()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "traefik",
        NamespaceProperty = "traefik"
      },
      Spec = new FluxHelmReleaseSpec
      {
        Interval = "10m",
        DependsOn = [
          new FluxDependsOn
          {
            Name = "cert-manager",
            Namespace = "cert-manager"
          }
        ],
        Chart = new FluxHelmReleaseSpecChart
        {
          Spec = new FluxHelmReleaseSpecChartSpec
          {
            Chart = "traefik",
            Version = "31.0.0",
            SourceRef = new FluxSourceRef
            {
              Kind = FluxSource.HelmRepository,
              Name = "traefik"
            }
          }
        },
        Values = new Dictionary<string, object>
        {
          ["tlsStore"] = new Dictionary<string, object>
          {
            ["default"] = new Dictionary<string, object>
            {
              ["defaultCertificate"] = new Dictionary<string, object>
              {
                ["secretName"] = "cluster-issuer-certificate-tls"
              }
            }
          }
        }
      }
    };
    await _helmReleaseGenerator.GenerateAsync(traefikHelmRelease, traefikHelmReleasePath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string traefikHelmRepositoryPath = Path.Combine(traefikPath, "helm-repository.yaml");
    if (File.Exists(traefikHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{traefikHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikHelmRepositoryPath}'");
    var traefikHelmRepository = new FluxHelmRepository()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "traefik",
        NamespaceProperty = "traefik"
      },
      Spec = new FluxHelmRepositorySpec
      {
        Url = new Uri("https://traefik.github.io/charts")
      }
    };
    await _helmRepositoryGenerator.GenerateAsync(traefikHelmRepository, traefikHelmRepositoryPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
