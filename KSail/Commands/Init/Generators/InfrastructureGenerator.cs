using KSail.Commands.Init.Models;

namespace KSail.Commands.Init.Generators;

class InfrastructureGenerator
{

  internal static async Task GenerateAsync(string name, Distribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateClusterInfrastructure(name, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionInfrastructure(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalInfrastructure(outputPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterInfrastructure(string name, string outputPath, CancellationToken cancellationToken)
  {
    string clusterInfrastructurePath = Path.Combine(outputPath, "clusters", name, "infrastructure");
    if (!Directory.Exists(clusterInfrastructurePath))
    {
      _ = Directory.CreateDirectory(clusterInfrastructurePath);
    }
    await GenerateClusterInfrastructureKustomization(clusterInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterInfrastructureKustomization(string clusterInfrastructurePath, CancellationToken cancellationToken)
  {
    string clusterInfrastructureKustomizationPath = Path.Combine(clusterInfrastructurePath, "kustomization.yaml");
    if (File.Exists(clusterInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterInfrastructureKustomizationPath}'");
    await File.WriteAllTextAsync(clusterInfrastructureKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionInfrastructure(Distribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionInfrastructurePath = Path.Combine(outputPath, "distributions", distribution.ToString(), "infrastructure");
    if (!Directory.Exists(distributionInfrastructurePath))
    {
      _ = Directory.CreateDirectory(distributionInfrastructurePath);
    }
    await GenerateDistributionInfrastructureKustomization(distributionInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionInfrastructureKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionInfrastructureKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionInfrastructureKustomizationPath}'");
    await File.WriteAllTextAsync(distributionInfrastructureKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalInfrastructure(string outputPath, CancellationToken cancellationToken)
  {
    string globalInfrastructurePath = Path.Combine(outputPath, "infrastructure");
    if (!Directory.Exists(globalInfrastructurePath))
    {
      _ = Directory.CreateDirectory(globalInfrastructurePath);
    }
    await GenerateGlobalInfrastructureKustomization(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
    await GenerateCertManager(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
    await GenerateTraefik(globalInfrastructurePath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalInfrastructureKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalInfrastructureKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalInfrastructureKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalInfrastructureKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalInfrastructureKustomizationPath}'");
    await File.WriteAllTextAsync(globalInfrastructureKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateCertManager(string outputPath, CancellationToken cancellationToken)
  {
    string certManagerPath = Path.Combine(outputPath, "cert-manager");
    if (!Directory.Exists(certManagerPath))
    {
      _ = Directory.CreateDirectory(certManagerPath);
    }

    string certManagerKustomizationPath = Path.Combine(certManagerPath, "kustomization.yaml");
    if (File.Exists(certManagerKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerKustomizationPath}'");
    await File.WriteAllTextAsync(certManagerKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);

    string certManagerNamespacePath = Path.Combine(certManagerPath, "namespace.yaml");
    if (File.Exists(certManagerNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerNamespacePath}'");
    await File.WriteAllTextAsync(certManagerNamespacePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string certManagerHelmReleasePath = Path.Combine(certManagerPath, "helm-release.yaml");
    if (File.Exists(certManagerHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerHelmReleasePath}'");
    await File.WriteAllTextAsync(certManagerHelmReleasePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string certManagerHelmRepositoryPath = Path.Combine(certManagerPath, "helm-repository.yaml");
    if (File.Exists(certManagerHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{certManagerHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{certManagerHelmRepositoryPath}'");
    await File.WriteAllTextAsync(certManagerHelmRepositoryPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateTraefik(string outputPath, CancellationToken cancellationToken)
  {
    string traefikPath = Path.Combine(outputPath, "traefik");
    if (!Directory.Exists(traefikPath))
    {
      _ = Directory.CreateDirectory(traefikPath);
    }

    string traefikKustomizationPath = Path.Combine(traefikPath, "kustomization.yaml");
    if (File.Exists(traefikKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{traefikKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikKustomizationPath}'");
    await File.WriteAllTextAsync(traefikKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);

    string traefikNamespacePath = Path.Combine(traefikPath, "namespace.yaml");
    if (File.Exists(traefikNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{traefikNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikNamespacePath}'");
    await File.WriteAllTextAsync(traefikNamespacePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string traefikHelmReleasePath = Path.Combine(traefikPath, "helm-release.yaml");
    if (File.Exists(traefikHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{traefikHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikHelmReleasePath}'");
    await File.WriteAllTextAsync(traefikHelmReleasePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string traefikHelmRepositoryPath = Path.Combine(traefikPath, "helm-repository.yaml");
    if (File.Exists(traefikHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{traefikHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{traefikHelmRepositoryPath}'");
    await File.WriteAllTextAsync(traefikHelmRepositoryPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
