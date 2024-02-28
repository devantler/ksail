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

    await GenerateFluxKustomizationFileAsync(Path.Combine(fluxSystemDirectory, "variables.yaml"),
    [
        new()
            {
              Name = "variables",
              Path = $"clusters/{clusterName}/variables"
            }
    ]);
    await GenerateKustomizationFileAsync(Path.Combine(clusterDirectory, "variables/kustomization.yaml"), ["variables.yaml", "variables-sensitive.sops.yaml"]);
    await GenerateConfigMapFileAsync(Path.Combine(clusterDirectory, "variables/variables.yaml"));
    await GenerateSecretFileAsync(Path.Combine(clusterDirectory, "variables/variables-sensitive.sops.yaml"));

    await GenerateFluxKustomizationFileAsync(Path.Combine(fluxSystemDirectory, "infrastructure.yaml"),
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
    await GenerateKustomizationFileAsync(Path.Combine(manifestsDirectory, "infrastructure/services/kustomization.yaml"), []);
    await GenerateKustomizationFileAsync(Path.Combine(manifestsDirectory, "infrastructure/configs/kustomization.yaml"), []);

    await GenerateFluxKustomizationFileAsync(Path.Combine(fluxSystemDirectory, "apps.yaml"),
    [
        new() {
            Name = "apps",
            Path = $"clusters/{clusterName}/apps",
            DependsOn = ["infrastructure-configs"]
        }
    ]);
    await GenerateKustomizationFileAsync(Path.Combine(manifestsDirectory, "apps/kustomization.yaml"), []);

    await GenerateK3dConfigFileAsync($"{clusterName}-k3d-config.yaml");
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
    Console.WriteLine("► Generating new key for SOPS");
    if (!keyExists && await _localSOPSProvisioner.CreateKeyAsync(KeyType.Age, clusterName, token) != 0)
    {
      Console.WriteLine("✕ Unexpected error occurred while creating a new Age key for SOPS.");
      return 1;
    }
    return 0;
  }

  async Task GenerateK3dConfigFileAsync(string filePath)
  {
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filePath)))
    {
      Console.WriteLine($"✕ A k3d-config.yaml file already exists at '{Directory.GetCurrentDirectory()}/{filePath}'. Skipping config creation.");
    }
    else
    {
      await CreateConfigAsync(clusterName);
    }
  }

  static async Task GenerateFluxKustomizationFileAsync(string filePath, List<FluxKustomizationContent> contents)
  {
    Console.WriteLine($"✚ Generating flux kustomization file '{filePath}'");
    if (!File.Exists(filePath))
    {
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
      string fileName = Path.GetFileName(filePath);
      Console.WriteLine($"✕ A {fileName} file already exists at '{filePath}'. Skipping creation.");
    }
  }

  static async Task GenerateKustomizationFileAsync(string filePath, List<string> resources, string @namespace = "")
  {
    Console.WriteLine($"✚ Generating kustomization file '{filePath}'");
    if (!File.Exists(filePath))
    {
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
      string fileName = Path.GetFileName(filePath);
      Console.WriteLine($"✕ A {fileName} file already exists at '{filePath}'. Skipping creation.");
    }
  }

  static async Task GenerateSecretFileAsync(string filePath)
  {
    Console.WriteLine($"✚ Creating variables-sensitive file '{filePath}'");
    const string variablesSensitiveYamlContent = """
      # You need to encrypt this file with SOPS manually.
      # ksail sops --encrypt variables-sensitive.sops.yaml
      apiVersion: v1
      kind: Secret
      metadata:
        name: variables-sensitive
      stringData: {}
      """;
    var variablesSensitiveYamlFile = File.Create(filePath) ?? throw new InvalidOperationException($"🚨 Could not create the variables-sensitive.sops.yaml file at '{filePath}'.");
    await variablesSensitiveYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesSensitiveYamlContent));
    await variablesSensitiveYamlFile.FlushAsync();
  }

  static async Task GenerateConfigMapFileAsync(string filePath)
  {
    Console.WriteLine($"✚ Creating variables file '{filePath}'");
    const string variablesYamlContent = """
      apiVersion: v1
      kind: ConfigMap
      metadata:
        name: variables
        namespace: flux-system
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
    Console.WriteLine($"✚ Creating config file './{clusterName}-k3d-config.yaml'");
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
