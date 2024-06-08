using System.Text;
using KSail.Models.Kubernetes;
using KSail.Models.Kubernetes.FluxKustomization;
using KSail.TemplateEngine;

namespace KSail.Commands.Init.Generators;

class KubernetesGenerator
{
  readonly Generator _generator = new(new TemplateEngine.TemplateEngine());

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
      );
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
      );
    }
    else
    {
      Console.WriteLine($"✓ Flux Kustomization '{filePath}' already exists");
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
      );
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
      stringData: {}
      """;
    var variablesSensitiveYamlFile = File.Create(filePath) ?? throw new InvalidOperationException($"🚨 Could not create '{filePath}'.");
    await variablesSensitiveYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesSensitiveYamlContent));
    await variablesSensitiveYamlFile.FlushAsync();
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
      data:
        cluster_domain: {clusterName}.local
        cluster_issuer_name: selfsigned-cluster-issuer
      """;
    var variablesYamlFile = File.Create(filePath) ?? throw new InvalidOperationException($"🚨 Could not create the variables.yaml file at {filePath}.");
    await variablesYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesYamlContent));
    await variablesYamlFile.FlushAsync();
  }
}
