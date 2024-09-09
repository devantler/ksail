using KSail.Commands.Init.Models;

namespace KSail.Commands.Init.Generators;

class AppsGenerator
{
  internal static async Task GenerateAsync(string name, Distribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    await GenerateClusterApps(name, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateDistributionApps(distribution, outputPath, cancellationToken).ConfigureAwait(false);
    await GenerateGlobalApps(outputPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterApps(string name, string outputPath, CancellationToken cancellationToken)
  {
    string clusterAppsPath = Path.Combine(outputPath, "clusters", name, "apps");
    if (!Directory.Exists(clusterAppsPath))
    {
      _ = Directory.CreateDirectory(clusterAppsPath);
    }
    await GenerateClusterAppsKustomization(clusterAppsPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateClusterAppsKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string clusterAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(clusterAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{clusterAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{clusterAppsKustomizationPath}'");
    await File.WriteAllTextAsync(clusterAppsKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionApps(Distribution distribution, string outputPath, CancellationToken cancellationToken)
  {
    string distributionAppsPath = Path.Combine(outputPath, "distributions", distribution.ToString(), "apps");
    if (!Directory.Exists(distributionAppsPath))
    {
      _ = Directory.CreateDirectory(distributionAppsPath);
    }
    await GenerateDistributionAppsKustomization(distributionAppsPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateDistributionAppsKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string distributionAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(distributionAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{distributionAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{distributionAppsKustomizationPath}'");
    await File.WriteAllTextAsync(distributionAppsKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalApps(string outputPath, CancellationToken cancellationToken)
  {
    string globalAppsPath = Path.Combine(outputPath, "apps");
    if (!Directory.Exists(globalAppsPath))
    {
      _ = Directory.CreateDirectory(globalAppsPath);
    }
    await GenerateGlobalAppsKustomization(globalAppsPath, cancellationToken).ConfigureAwait(false);
    await GeneratePodinfo(globalAppsPath, cancellationToken).ConfigureAwait(false);
  }

  static async Task GenerateGlobalAppsKustomization(string outputPath, CancellationToken cancellationToken)
  {
    string globalAppsKustomizationPath = Path.Combine(outputPath, "kustomization.yaml");
    if (File.Exists(globalAppsKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{globalAppsKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{globalAppsKustomizationPath}'");
    await File.WriteAllTextAsync(globalAppsKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }

  static async Task GeneratePodinfo(string outputPath, CancellationToken cancellationToken)
  {
    string podinfoPath = Path.Combine(outputPath, "podinfo");
    if (!Directory.Exists(podinfoPath))
    {
      _ = Directory.CreateDirectory(podinfoPath);
    }

    string podinfoKustomizationPath = Path.Combine(podinfoPath, "kustomization.yaml");
    if (File.Exists(podinfoKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoKustomizationPath}'");
    await File.WriteAllTextAsync(podinfoKustomizationPath, string.Empty, cancellationToken).ConfigureAwait(false);

    string podinfoNamespacePath = Path.Combine(podinfoPath, "namespace.yaml");
    if (File.Exists(podinfoNamespacePath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoNamespacePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoNamespacePath}'");
    await File.WriteAllTextAsync(podinfoNamespacePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string podinfoHelmReleasePath = Path.Combine(podinfoPath, "helm-release.yaml");
    if (File.Exists(podinfoHelmReleasePath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoHelmReleasePath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoHelmReleasePath}'");
    await File.WriteAllTextAsync(podinfoHelmReleasePath, string.Empty, cancellationToken).ConfigureAwait(false);

    string podinfoHelmRepositoryPath = Path.Combine(podinfoPath, "helm-repository.yaml");
    if (File.Exists(podinfoHelmRepositoryPath))
    {
      Console.WriteLine($"✔ Skipping '{podinfoHelmRepositoryPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{podinfoHelmRepositoryPath}'");
    await File.WriteAllTextAsync(podinfoHelmRepositoryPath, string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
