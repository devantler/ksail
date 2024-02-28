using System.Text;
using KSail.Generators;
using KSail.Models.Kubernetes.FluxKustomization;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(string clusterName, string manifestsDirectory) : IDisposable
{
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();

  internal async Task<int> HandleAsync(CancellationToken token)
  {
    string clusterDirectory = Path.Combine(manifestsDirectory, "clusters", clusterName);
    string fluxSystemDirectory = Path.Combine(clusterDirectory, "flux-system");

    await GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "variables.yaml"),
    [
        new()
            {
              Name = "variables",
              Path = $"clusters/{clusterName}/variables",
              PostBuild = new()
              {
                SubstituteFrom = []
              }
            }
    ]);
    await GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "infrastructure.yaml"),
    [
        new FluxKustomizationContent
        {
            Name = "infrastructure-services",
            Path = "infrastructure/services",
            DependsOn = ["variables"]
        },
        new FluxKustomizationContent
        {
            Name = "infrastructure-configs",
            Path = "infrastructure/configs",
            DependsOn = ["infrastructure-services"]
        }
    ]);
    await GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "apps.yaml"),
    [
        new() {
            Name = "apps",
            Path = "apps",
            DependsOn = ["infrastructure-configs"]
        }
    ]);
    await GenerateKustomizationAsync(Path.Combine(clusterDirectory, "variables/kustomization.yaml"), ["variables.yaml", "variables-sensitive.sops.yaml"], "flux-system");
    await GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/services/kustomization.yaml"),
    [
      "github.com/devantler/oci-registry//k8s/cert-manager",
      "github.com/devantler/oci-registry//k8s/traefik"
    ]);
    await GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/configs/kustomization.yaml"),
    [
      "raw.githubusercontent.com/devantler/oci-registry/main/k8s/cert-manager/certificates/cluster-issuer-certificate.yaml",
      "raw.githubusercontent.com/devantler/oci-registry/main/k8s/cert-manager/cluster-issuers/selfsigned-cluster-issuer.yaml"
    ]);
    await GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "apps/kustomization.yaml"),
    [
      "github.com/stefanprodan/podinfo//kustomize"
    ]);

    await GenerateConfigMapAsync(Path.Combine(clusterDirectory, "variables/variables.yaml"));
    await GenerateSecretAsync(Path.Combine(clusterDirectory, "variables/variables-sensitive.sops.yaml"));

    await GenerateK3dConfigAsync($"{clusterName}-k3d-config.yaml");
    //TODO: await GenerateKSailConfigFileAsync($"{clusterName}-ksail-config.yaml");
    //TODO: await GenerateSOPSConfigFileAsync(".sops.yaml");

    //TODO: Move this method to a generator. Provisioning should not generate files.
    return await ProvisionSOPSKey(clusterName, token);
  }

  async Task<int> ProvisionSOPSKey(string clusterName, CancellationToken token)
  {
    var (keyExistsExitCode, keyExists) = await _localSOPSProvisioner.KeyExistsAsync(KeyType.Age, clusterName, token);
    if (keyExistsExitCode != 0)
    {
      Console.WriteLine("✕ Unexpected error occurred while checking for an existing Age key for SOPS.");
      return 1;
    }
    Console.WriteLine("✚ Generating SOPS");
    if (!keyExists && await _localSOPSProvisioner.CreateKeyAsync(KeyType.Age, clusterName, token) != 0)
    {
      Console.WriteLine("✕ Unexpected error occurred while creating a new Age key for SOPS.");
      return 1;
    }
    return 0;
  }

  async Task GenerateK3dConfigAsync(string filePath)
  {
    if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filePath)))
    {
      await CreateConfigAsync(clusterName);
    }
  }

  static async Task GenerateFluxKustomizationAsync(string filePath, List<FluxKustomizationContent> contents)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating Flux Kustomization '{filePath}'");
      var fluxKustomization = new FluxKustomization
      {
        Content = contents
      };
      await Generator.GenerateAsync(
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

  static async Task GenerateKustomizationAsync(string filePath, List<string> resources, string @namespace = "")
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating Kustomization '{filePath}'");
      await Generator.GenerateAsync(
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

  static async Task GenerateSecretAsync(string filePath)
  {
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

  static async Task GenerateConfigMapAsync(string filePath)
  {
    Console.WriteLine($"✚ Generating ConfigMap '{filePath}'");
    const string variablesYamlContent = """
      apiVersion: v1
      kind: ConfigMap
      metadata:
        name: variables
      data:
        cluster_domain: test
        cluster_issuer_name: selfsigned-cluster-issuer
      """;
    var variablesYamlFile = File.Create(filePath) ?? throw new InvalidOperationException($"🚨 Could not create the variables.yaml file at {filePath}.");
    await variablesYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesYamlContent));
    await variablesYamlFile.FlushAsync();
  }

  static async Task CreateConfigAsync(string clusterName)
  {
    Console.WriteLine($"✚ Generating K3d Config './{clusterName}-k3d-config.yaml'");
    string configPath = Path.Combine(Directory.GetCurrentDirectory(), $"{clusterName}-k3d-config.yaml");
    string configContent = $"""
      apiVersion: k3d.io/v1alpha5
      kind: Simple
      metadata:
        name: {clusterName}
      volumes:
        - volume: k3d-{clusterName}-storage:/var/lib/rancher/k3s/storage
      network: k3d-{clusterName}
      options:
        k3s:
          extraArgs:
            - arg: "--disable=traefik"
              nodeFilters:
                - server:*
      registries:
        config: |
          mirrors:
            "docker.io":
              endpoint:
                - http://host.k3d.internal:5001
            "registry.k8s.io":
              endpoint:
                - http://host.k3d.internal:5002
            "gcr.io":
              endpoint:
                - http://host.k3d.internal:5003
            "ghcr.io":
              endpoint:
                - http://host.k3d.internal:5004
            "quay.io":
              endpoint:
                - http://host.k3d.internal:5005
            "mcr.microsoft.com":
              endpoint:
                - http://host.k3d.internal:5006
      """;
    var configFile = File.Create(configPath) ?? throw new InvalidOperationException($"🚨 Could not create the config file at {configPath}.");
    await configFile.WriteAsync(Encoding.UTF8.GetBytes(configContent));
    await configFile.FlushAsync();
  }

  public void Dispose()
  {
    _localSOPSProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
