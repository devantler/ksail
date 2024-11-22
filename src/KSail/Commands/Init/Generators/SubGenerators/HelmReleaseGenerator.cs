using Devantler.KubernetesGenerator.CertManager;
using Devantler.KubernetesGenerator.CertManager.Models;
using Devantler.KubernetesGenerator.CertManager.Models.IssuerRef;
using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Dependencies;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class HelmReleaseGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  readonly NamespaceGenerator _namespaceGenerator = new();
  readonly CertManagerCertificateGenerator _certificateGenerator = new();
  readonly CertManagerClusterIssuerGenerator _clusterIssuerGenerator = new();
  readonly FluxHelmReleaseGenerator _helmReleaseGenerator = new();
  readonly FluxHelmRepositoryGenerator _helmRepositoryGenerator = new();

  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string appsPath = Path.Combine(config.Spec.Project.ManifestsDirectory, config.Spec.CLI.InitOptions.KustomizeHooks.Last(), "apps");
    string infrastructurePath = Path.Combine(config.Spec.Project.ManifestsDirectory, config.Spec.CLI.InitOptions.KustomizeHooks.Last(), "infrastructure");
    string infrastructureControllersPath = Path.Combine(infrastructurePath, "controllers");
    if (!Directory.Exists(appsPath))
      _ = Directory.CreateDirectory(appsPath);
    if (!Directory.Exists(infrastructurePath))
      _ = Directory.CreateDirectory(infrastructurePath);
    if (!Directory.Exists(infrastructureControllersPath))
      _ = Directory.CreateDirectory(infrastructureControllersPath);

    await GeneratePodinfo(appsPath, cancellationToken).ConfigureAwait(false);
    await GenerateCertManager(infrastructureControllersPath, cancellationToken).ConfigureAwait(false);
    await GenerateTraefik(infrastructureControllersPath, cancellationToken).ConfigureAwait(false);
    await GenerateClusterIssuers(infrastructurePath, cancellationToken).ConfigureAwait(false);
    await GenerateCertificates(infrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateCertificates(string infrastructurePath, CancellationToken cancellationToken)
  {
    string certificatesPath = Path.Combine(infrastructurePath, "certificates");
    if (!Directory.Exists(certificatesPath))
      _ = Directory.CreateDirectory(certificatesPath);

    await GenerateCertificatesKustomization(certificatesPath, cancellationToken).ConfigureAwait(false);
    await GenerateClusterIssuerCertificate(certificatesPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterIssuerCertificate(string certificatesPath, CancellationToken cancellationToken)
  {
    string clusterIssuerCertificatePath = Path.Combine(certificatesPath, "cluster-issuer-certificate.yaml");
    if (File.Exists(clusterIssuerCertificatePath))
    {
      Console.WriteLine($"✔ skipping '{clusterIssuerCertificatePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{clusterIssuerCertificatePath}'");
    var clusterIssuerCertificate = new CertManagerCertificate
    {
      Metadata = new V1ObjectMeta
      {
        Name = "cluster-issuer-certificate",
        NamespaceProperty = "traefik"
      },
      Spec = new CertManagerCertificateSpec
      {
        SecretName = "cluster-issuer-certificate-tls",
        DnsNames = ["k8s.local", "*.k8s.local"],
        IssuerRef = new CertManagerIssuerRef()
        {
          Name = "selfsigned-cluster-issuer",
          Kind = "ClusterIssuer"
        }
      }
    };
    await _certificateGenerator.GenerateAsync(clusterIssuerCertificate, clusterIssuerCertificatePath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateCertificatesKustomization(string certificatesPath, CancellationToken cancellationToken)
  {
    string certificatesKustomizationPath = Path.Combine(certificatesPath, "kustomization.yaml");
    if (File.Exists(certificatesKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{certificatesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{certificatesKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "cluster-issuer-certificate.yaml"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, certificatesKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterIssuers(string infrastructurePath, CancellationToken cancellationToken)
  {
    string ClusterIssuersPath = Path.Combine(infrastructurePath, "cluster-issuers");
    if (!Directory.Exists(ClusterIssuersPath))
      _ = Directory.CreateDirectory(ClusterIssuersPath);

    await GenerateClusterIssuersKustomization(ClusterIssuersPath, cancellationToken).ConfigureAwait(false);
    await GenerateSelfSignedClusterIssuer(ClusterIssuersPath, cancellationToken).ConfigureAwait(false);

  }

  async Task GenerateSelfSignedClusterIssuer(string clusterIssuersPath, CancellationToken cancellationToken)
  {
    string selfSignedClusterIssuerPath = Path.Combine(clusterIssuersPath, "selfsigned-cluster-issuer.yaml");
    if (File.Exists(selfSignedClusterIssuerPath))
    {
      Console.WriteLine($"✔ skipping '{selfSignedClusterIssuerPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{selfSignedClusterIssuerPath}'");
    var selfSignedClusterIssuer = new CertManagerClusterIssuer
    {
      Metadata = new V1ObjectMeta
      {
        Name = "selfsigned-cluster-issuer",
        NamespaceProperty = "cert-manager"
      },
      Spec = new CertManagerClusterIssuerSpec
      {
        SelfSigned = new()
      }
    };
    await _clusterIssuerGenerator.GenerateAsync(selfSignedClusterIssuer, selfSignedClusterIssuerPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterIssuersKustomization(string selfSignedClusterIssuerPath, CancellationToken cancellationToken)
  {
    string selfSignedClusterIssuerKustomizationPath = Path.Combine(selfSignedClusterIssuerPath, "kustomization.yaml");
    if (File.Exists(selfSignedClusterIssuerKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{selfSignedClusterIssuerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{selfSignedClusterIssuerKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "selfsigned-cluster-issuer.yaml",
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, selfSignedClusterIssuerKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodinfo(string outputPath, CancellationToken cancellationToken = default)
  {
    string podinfoPath = Path.Combine(outputPath, "podinfo");
    if (!Directory.Exists(podinfoPath))
      _ = Directory.CreateDirectory(podinfoPath);

    await GeneratePodInfoKustomization(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoNamespace(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoHelmRelease(podinfoPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodInfoHelmRepository(podinfoPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GeneratePodInfoKustomization(string podinfoPath, CancellationToken cancellationToken = default)
  {
    string podinfoKustomizationPath = Path.Combine(podinfoPath, "kustomization.yaml");
    if (File.Exists(podinfoKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{podinfoKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{podinfoKustomizationPath}'");
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

  async Task GeneratePodInfoNamespace(string podinfoPath, CancellationToken cancellationToken = default)
  {
    string podinfoNamespacePath = Path.Combine(podinfoPath, "namespace.yaml");
    if (File.Exists(podinfoNamespacePath))
    {
      Console.WriteLine($"✔ skipping '{podinfoNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{podinfoNamespacePath}'");
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

  async Task GeneratePodInfoHelmRelease(string podinfoPath, CancellationToken cancellationToken = default)
  {
    string podinfoHelmReleasePath = Path.Combine(podinfoPath, "helm-release.yaml");
    if (File.Exists(podinfoHelmReleasePath))
    {
      Console.WriteLine($"✔ skipping '{podinfoHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{podinfoHelmReleasePath}'");
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

  async Task GeneratePodInfoHelmRepository(string podinfoPath, CancellationToken cancellationToken = default)
  {
    string podinfoHelmRepositoryPath = Path.Combine(podinfoPath, "helm-repository.yaml");
    if (File.Exists(podinfoHelmRepositoryPath))
    {
      Console.WriteLine($"✔ skipping '{podinfoHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{podinfoHelmRepositoryPath}'");
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


  async Task GenerateCertManager(string outputPath, CancellationToken cancellationToken = default)
  {
    string certManagerPath = Path.Combine(outputPath, "cert-manager");
    if (!Directory.Exists(certManagerPath))
      _ = Directory.CreateDirectory(certManagerPath);

    string certManagerKustomizationPath = Path.Combine(certManagerPath, "kustomization.yaml");
    if (File.Exists(certManagerKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{certManagerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{certManagerKustomizationPath}'");
    var certManagerKustomization = new KustomizeKustomization()
    {
      Resources = [
        "namespace.yaml",
        "helm-release.yaml",
        "helm-repository.yaml"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(certManagerKustomization, certManagerKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string certManagerNamespacePath = Path.Combine(certManagerPath, "namespace.yaml");
    if (File.Exists(certManagerNamespacePath))
    {
      Console.WriteLine($"✔ skipping '{certManagerNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{certManagerNamespacePath}'");
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
      Console.WriteLine($"✔ skipping '{certManagerHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{certManagerHelmReleasePath}'");
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
      Console.WriteLine($"✔ skipping '{certManagerHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{certManagerHelmRepositoryPath}'");
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

  async Task GenerateTraefik(string outputPath, CancellationToken cancellationToken = default)
  {
    string traefikPath = Path.Combine(outputPath, "traefik");
    if (!Directory.Exists(traefikPath))
      _ = Directory.CreateDirectory(traefikPath);

    string traefikKustomizationPath = Path.Combine(traefikPath, "kustomization.yaml");
    if (File.Exists(traefikKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{traefikKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{traefikKustomizationPath}'");
    var traefikKustomization = new KustomizeKustomization()
    {
      Resources = [
        "namespace.yaml",
        "helm-release.yaml",
        "helm-repository.yaml"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(traefikKustomization, traefikKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string traefikNamespacePath = Path.Combine(traefikPath, "namespace.yaml");
    if (File.Exists(traefikNamespacePath))
    {
      Console.WriteLine($"✔ skipping '{traefikNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{traefikNamespacePath}'");
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
      Console.WriteLine($"✔ skipping '{traefikHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{traefikHelmReleasePath}'");
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
          ["service"] = new Dictionary<string, object>
          {
            ["type"] = "ClusterIP"
          },
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
      Console.WriteLine($"✔ skipping '{traefikHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{traefikHelmRepositoryPath}'");
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
