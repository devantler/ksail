using System.Text;
using KSail.Models.Kubernetes;
using KSail.Models.Kubernetes.FluxKustomization;
using Devantler.TemplateEngine;

namespace KSail.Commands.Init.Generators;

class KubernetesGenerator
{
  readonly Generator _generator = new(new TemplateEngine());

  internal async Task GenerateNamespaceAsync(string filePath, string name)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating Namespace '{filePath}'");
      await _generator.GenerateAsync(
        filePath,
        $"{AppDomain.CurrentDomain.BaseDirectory}/assets/templates/kubernetes/namespace.sbn",
        new Namespace
        {
          Name = name
        }
      ).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine($"✓ Namespace '{filePath}' already exists");
    }
  }

  internal async Task GenerateFluxKustomizationAsync(string filePath, List<FluxKustomizationContent> contents)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating Flux Kustomization '{filePath}'");
      var fluxKustomization = new FluxKustomization
      {
        Content = contents
      };
      await _generator.GenerateAsync(
          filePath,
          $"{AppDomain.CurrentDomain.BaseDirectory}/assets/templates/kubernetes/flux-kustomization.sbn",
          fluxKustomization
      ).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine($"✓ Flux Kustomization '{filePath}' already exists");
    }
  }

  internal async Task GenerateOCIRepositoryAsync(string filePath, OCIRepository ociRepository)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating OCI Repository '{filePath}'");
      await _generator.GenerateAsync(
          filePath,
          $"{AppDomain.CurrentDomain.BaseDirectory}/assets/templates/kubernetes/oci-repository.sbn",
          ociRepository
      ).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine($"✓ OCI Repository '{filePath}' already exists");
    }
  }

  internal async Task GenerateKustomizationAsync(string filePath, List<string> resources, string @namespace = "")
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating Kustomization '{filePath}'");
      await _generator.GenerateAsync(
        filePath,
        $"{AppDomain.CurrentDomain.BaseDirectory}/assets/templates/kubernetes/kustomization.sbn",
        new Kustomization
        {
          Namespace = @namespace,
          Resources = resources
        }
      ).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine($"✓ Kustomization '{filePath}' already exists");
    }
  }

  internal static async Task GenerateSecretAsync(string filePath)
  {
    if (File.Exists(filePath))
    {
      Console.WriteLine($"✓ Secret '{filePath}' already exists");
      return;
    }
    Console.WriteLine($"✚ Generating Secret '{filePath}'");
    const string variablesSensitiveYamlContent = """
      # You need to encrypt this file with SOPS manually.
      # ksail sops --encrypt variables-sensitive.sops.yaml
      apiVersion: v1
      kind: Secret
      metadata:
        name: variables-sensitive
        namespace: flux-system
      stringData: {}
      """;
    string? directoryPath = Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException($"🚨 Could not get the directory path of '{filePath}'.");
    if (!Directory.Exists(directoryPath))
    {
      _ = Directory.CreateDirectory(directoryPath);
    }
    using var variablesSensitiveYamlFile = File.Create(filePath);
    await variablesSensitiveYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesSensitiveYamlContent)).ConfigureAwait(false);
    await variablesSensitiveYamlFile.FlushAsync().ConfigureAwait(false);
  }

  internal static async Task GenerateConfigMapAsync(string filePath, string clusterName)
  {
    if (File.Exists(filePath))
    {
      Console.WriteLine($"✓ ConfigMap '{filePath}' already exists");
      return;
    }
    Console.WriteLine($"✚ Generating ConfigMap '{filePath}'");
    string variablesYamlContent = $"""
      apiVersion: v1
      kind: ConfigMap
      metadata:
        name: variables
        namespace: flux-system
      data:
        cluster_domain: {clusterName}.local
        cluster_issuer_name: selfsigned-cluster-issuer
      """;
    string? directoryPath = Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException($"🚨 Could not get the directory path of '{filePath}'.");
    if (!Directory.Exists(directoryPath))
    {
      _ = Directory.CreateDirectory(directoryPath);
    }
    using var variablesYamlFile = File.Create(filePath);
    await variablesYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesYamlContent)).ConfigureAwait(false);
    await variablesYamlFile.FlushAsync().ConfigureAwait(false);
  }
}
