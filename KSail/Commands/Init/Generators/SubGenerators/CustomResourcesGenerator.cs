using System.Globalization;
using Devantler.KubernetesGenerator.CertManager;
using Devantler.KubernetesGenerator.CertManager.Models;
using Devantler.KubernetesGenerator.CertManager.Models.IssuerRef;
using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using k8s.Models;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class CustomResourcesGenerator
{
  readonly KustomizeKustomizationGenerator _kustomizationGenerator = new();
  readonly CertManagerCertificateGenerator _certificateGenerator = new();
  readonly CertManagerClusterIssuerGenerator _clusterIssuerGenerator = new();

  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    await GenerateClusterCustomResources(config, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionCustomResources(config, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalCustomResources(config, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterCustomResources(KSailCluster config, CancellationToken cancellationToken)
  {
    string clusterCustomResourcesPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "clusters", config.Metadata.Name, "custom-resources");
    if (!Directory.Exists(clusterCustomResourcesPath))
      _ = Directory.CreateDirectory(clusterCustomResourcesPath);
    await GenerateClusterCustomResourcesKustomization(config.Spec.Distribution, clusterCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateClusterCustomResourcesKustomization(KSailKubernetesDistribution distribution, string clusterCustomResourcesPath, CancellationToken cancellationToken)
  {
    string clusterCustomResourcesKustomizationPath = Path.Combine(clusterCustomResourcesPath, "kustomization.yaml");
    if (File.Exists(clusterCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterCustomResourcesKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        $"../../../distributions/{distribution.ToString().ToLower(CultureInfo.CurrentCulture)}/custom-resources"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, clusterCustomResourcesKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionCustomResources(KSailCluster config, CancellationToken cancellationToken)
  {
    string distributionCustomResourcesPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "distributions", config.Spec.Distribution.ToString().ToLower(CultureInfo.CurrentCulture), "custom-resources");
    if (!Directory.Exists(distributionCustomResourcesPath))
      _ = Directory.CreateDirectory(distributionCustomResourcesPath);
    await GenerateDistributionCustomResourcesKustomization(distributionCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateDistributionCustomResourcesKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionCustomResourcesKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionCustomResourcesKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "../../../custom-resources"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, distributionCustomResourcesKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalCustomResources(KSailCluster config, CancellationToken cancellationToken)
  {
    string globalCustomResourcesPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "custom-resources");
    if (!Directory.Exists(globalCustomResourcesPath))
      _ = Directory.CreateDirectory(globalCustomResourcesPath);
    await GenerateGlobalCustomResourcesKustomization(globalCustomResourcesPath, cancellationToken).ConfigureAwait(false);
    await GenerateSelfSignedClusterIssuer(globalCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateGlobalCustomResourcesKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalCustomResourcesKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalCustomResourcesKustomizationPath}'");
    var kustomization = new KustomizeKustomization
    {
      Resources =
      [
        "selfsigned-cluster-issuer"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(kustomization, globalCustomResourcesKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateSelfSignedClusterIssuer(string outputPath, CancellationToken cancellationToken)
  {
    string selfSignedClusterIssuerPath = Path.Combine(outputPath, "selfsigned-cluster-issuer");
    if (!Directory.Exists(selfSignedClusterIssuerPath))
      _ = Directory.CreateDirectory(selfSignedClusterIssuerPath);

    string selfSignedClusterIssuerKustomizationPath = Path.Combine(selfSignedClusterIssuerPath, "kustomization.yaml");
    if (File.Exists(selfSignedClusterIssuerKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerKustomizationPath}'");
    var selfSignedClusterIssuerKustomization = new KustomizeKustomization
    {
      Resources =
      [
        "certificate.yaml",
        "cluster-issuer.yaml"
      ]
    };
    await _kustomizationGenerator.GenerateAsync(selfSignedClusterIssuerKustomization, selfSignedClusterIssuerKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string selfSignedClusterIssuerCertificatePath = Path.Combine(selfSignedClusterIssuerPath, "certificate.yaml");
    if (File.Exists(selfSignedClusterIssuerCertificatePath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerCertificatePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerCertificatePath}'");
    var certificate = new CertManagerCertificate
    {
      Metadata = new V1ObjectMeta
      {
        Name = "selfsigned-cluster-issuer",
        NamespaceProperty = "cert-manager"
      },
      Spec = new CertManagerCertificateSpec
      {
        SecretName = "cluster-issuer-certificate-tls",
        DnsNames =
        [
          "${cluster_domain}",
          "*.${cluster_domain}"
        ],
        IssuerRef = new CertManagerIssuerRef
        {
          Name = "selfsigned-cluster-issuer",
          Kind = "ClusterIssuer"
        }
      }
    };
    await _certificateGenerator.GenerateAsync(certificate, selfSignedClusterIssuerCertificatePath, cancellationToken: cancellationToken).ConfigureAwait(false);

    string selfSignedClusterIssuerClusterIssuerPath = Path.Combine(selfSignedClusterIssuerPath, "cluster-issuer.yaml");
    if (File.Exists(selfSignedClusterIssuerClusterIssuerPath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerClusterIssuerPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerClusterIssuerPath}'");
    var clusterIssuer = new CertManagerClusterIssuer
    {
      Metadata = new V1ObjectMeta
      {
        Name = "selfsigned-cluster-issuer",
        NamespaceProperty = "cert-manager"
      },
      Spec = new CertManagerClusterIssuerSpec
      {
        SelfSigned = new object()
      }
    };
    await _clusterIssuerGenerator.GenerateAsync(clusterIssuer, selfSignedClusterIssuerClusterIssuerPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
