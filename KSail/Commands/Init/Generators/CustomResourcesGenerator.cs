using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Commands.Init.Generators;

class CustomResourcesGenerator
{

  internal static async Task GenerateAsync(string name, KSailKubernetesDistribution distribution, string k8sPath, CancellationToken cancellationToken)
  {
    await GenerateClusterCustomResources(name, k8sPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionCustomResources(distribution, k8sPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalCustomResources(k8sPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterCustomResources(string name, string outputPath, CancellationToken cancellationToken)
  {
    string clusterCustomResourcesPath = Path.Combine(outputPath, "clusters", name, "custom-resources");
    if (!Directory.Exists(clusterCustomResourcesPath))
    {
      _ = Directory.CreateDirectory(clusterCustomResourcesPath);
    }
    await GenerateClusterCustomResourcesKustomization(clusterCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterCustomResourcesKustomization(string clusterCustomResourcesPath, CancellationToken cancellationToken)
  {
    string clusterCustomResourcesKustomizationPath = Path.Combine(clusterCustomResourcesPath, "kustomization.yaml");
    if (File.Exists(clusterCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterCustomResourcesKustomizationPath}'");
    await File.WriteAllTextAsync(clusterCustomResourcesKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionCustomResources(KSailKubernetesDistribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionCustomResourcesPath = Path.Combine(outputPath, "distributions", distribution.ToString(), "custom-resources");
    if (!Directory.Exists(distributionCustomResourcesPath))
    {
      _ = Directory.CreateDirectory(distributionCustomResourcesPath);
    }
    await GenerateDistributionCustomResourcesKustomization(distributionCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionCustomResourcesKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionCustomResourcesKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionCustomResourcesKustomizationPath}'");
    await File.WriteAllTextAsync(distributionCustomResourcesKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalCustomResources(string outputPath, CancellationToken cancellationToken)
  {
    string globalCustomResourcesPath = Path.Combine(outputPath, "custom-resources");
    if (!Directory.Exists(globalCustomResourcesPath))
    {
      _ = Directory.CreateDirectory(globalCustomResourcesPath);
    }
    await GenerateGlobalCustomResourcesKustomization(globalCustomResourcesPath, cancellationToken).ConfigureAwait(false);
    await GenerateSelfSignedClusterIssuer(globalCustomResourcesPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalCustomResourcesKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalCustomResourcesKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalCustomResourcesKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalCustomResourcesKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalCustomResourcesKustomizationPath}'");
    await File.WriteAllTextAsync(globalCustomResourcesKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateSelfSignedClusterIssuer(string outputPath, CancellationToken cancellationToken)
  {
    string selfSignedClusterIssuerPath = Path.Combine(outputPath, "selfsigned-cluster-issuer");
    if (!Directory.Exists(selfSignedClusterIssuerPath))
    {
      _ = Directory.CreateDirectory(selfSignedClusterIssuerPath);
    }

    string selfSignedClusterIssuerKustomizationPath = Path.Combine(selfSignedClusterIssuerPath, "kustomization.yaml");
    if (File.Exists(selfSignedClusterIssuerKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerKustomizationPath}'");
    await File.WriteAllTextAsync(selfSignedClusterIssuerKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);

    string selfSignedClusterIssuerCertificatePath = Path.Combine(selfSignedClusterIssuerPath, "certificate.yaml");
    if (File.Exists(selfSignedClusterIssuerCertificatePath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerCertificatePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerCertificatePath}'");
    await File.WriteAllTextAsync(selfSignedClusterIssuerCertificatePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string selfSignedClusterIssuerClusterIssuerPath = Path.Combine(selfSignedClusterIssuerPath, "cluster-issuer.yaml");
    if (File.Exists(selfSignedClusterIssuerClusterIssuerPath))
    {
      Console.WriteLine($"✔ Skipping '{selfSignedClusterIssuerClusterIssuerPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{selfSignedClusterIssuerClusterIssuerPath}'");
    await File.WriteAllTextAsync(selfSignedClusterIssuerClusterIssuerPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
