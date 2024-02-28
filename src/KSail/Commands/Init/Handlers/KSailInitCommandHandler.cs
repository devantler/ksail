using System.Text;
using KSail.Generators;
using KSail.Generators.Kubernetes.Models.FluxKustomization;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler : IDisposable
{
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();
  internal async Task<int> HandleAsync(string clusterName, string manifests, CancellationToken token)
  {
    string clusterDirectory = Path.Combine(manifests, "clusters", clusterName);

    var variablesFluxKustomization = new FluxKustomization
    {
      Content = [
        new FluxKustomizationContent {
          Name = "variables",
          Path = $"./clusters/{clusterName}/variables",
        }
      ]
    };
    await Generator.GenerateAsync(
      Path.Combine(clusterDirectory, "flux-system", "variables.yaml"),
      $"{AppDomain.CurrentDomain.BaseDirectory}/templates/flux/kustomization.sbn",
      variablesFluxKustomization
    );

    var infrastructureFluxKustomization = new FluxKustomization
    {
      Content = [
        new FluxKustomizationContent {
          Name = "infrastructure-services",
          Path = "./infrastructure/services",
        },
        new FluxKustomizationContent {
          Name = "infrastructure-configs",
          Path = "./infrastructure/configs",
        }
      ]
    };
    await Generator.GenerateAsync(
      Path.Combine(clusterDirectory, "flux-system", "infrastructure.yaml"),
      $"{AppDomain.CurrentDomain.BaseDirectory}/templates/flux/kustomization.sbn",
      infrastructureFluxKustomization
    );

    var appsFluxKustomization = new FluxKustomization
    {
      Content = [
        new FluxKustomizationContent {
          Name = "apps",
          Path = $"./clusters/{clusterName}/apps",
        }
      ]
    };
    await Generator.GenerateAsync(
      Path.Combine(clusterDirectory, "flux-system", "apps.yaml"),
      $"{AppDomain.CurrentDomain.BaseDirectory}/templates/flux/kustomization.sbn",
      appsFluxKustomization
    );

    // TODO: Migrate this code to the generator
    await CreateKustomizationsAsync(clusterDirectory);

    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), $"{clusterName}-k3d-config.yaml")))
    {
      Console.WriteLine($"✕ A k3d-config.yaml file already exists at '{Directory.GetCurrentDirectory()}/{clusterName}-k3d-config.yaml'. Skipping config creation.");
    }
    else
    {
      await CreateConfigAsync(clusterName);
    }
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

    Console.WriteLine($"✔ Successfully initialized a new K8s GitOps project named '{clusterName}'.");
    Console.WriteLine();
    return 0;
  }

  static async Task CreateKustomizationsAsync(string clusterDirectory)
  {
    Console.WriteLine($"✚ Creating infrastructure-services kustomization '{clusterDirectory}/infrastructure/services/kustomization.yaml'");
    string infrastructureServicesDirectory = Path.Combine(clusterDirectory, "infrastructure/services");
    _ = Directory.CreateDirectory(infrastructureServicesDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure directory at {infrastructureServicesDirectory}.");
    const string infrastructureKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      resources:
        - https://github.com/devantler/oci-registry//k8s/cert-manager?ref=v0.0.3
        - https://github.com/devantler/oci-registry//k8s/traefik?ref=v0.0.3
      """;
    string infrastructureServicesKustomizationPath = Path.Combine(infrastructureServicesDirectory, "kustomization.yaml");
    var infrastructureServicesKustomizationFile = File.Create(infrastructureServicesKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure kustomization.yaml file at {infrastructureServicesKustomizationPath}.");
    await infrastructureServicesKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(infrastructureKustomizationContent));
    await infrastructureServicesKustomizationFile.FlushAsync();

    Console.WriteLine($"✚ Creating infrastructure-configs kustomization '{clusterDirectory}/infrastructure/configs/kustomization.yaml'");
    string infrastructureConfigsDirectory = Path.Combine(clusterDirectory, "infrastructure/configs");
    _ = Directory.CreateDirectory(infrastructureConfigsDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure directory at {infrastructureConfigsDirectory}.");
    const string infrastructureConfigsKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      resources:
        - https://raw.githubusercontent.com/devantler/oci-registry/v0.0.2/k8s/cert-manager/certificates/cluster-issuer-certificate.yaml
        - https://raw.githubusercontent.com/devantler/oci-registry/v0.0.2/k8s/cert-manager/cluster-issuers/selfsigned-cluster-issuer.yaml
      """;
    string infrastructureConfigsKustomizationPath = Path.Combine(infrastructureConfigsDirectory, "kustomization.yaml");
    var infrastructureConfigsKustomizationFile = File.Create(infrastructureConfigsKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure kustomization.yaml file at {infrastructureConfigsKustomizationPath}.");
    await infrastructureConfigsKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(infrastructureConfigsKustomizationContent));
    await infrastructureConfigsKustomizationFile.FlushAsync();

    Console.WriteLine($"✚ Creating variables kustomization '{clusterDirectory}/variables/kustomization.yaml'");
    string variablesDirectory = Path.Combine(clusterDirectory, "variables");
    _ = Directory.CreateDirectory(variablesDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the variables directory at {variablesDirectory}.");
    const string variablesKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      namespace: flux-system
      resources:
        - variables.yaml
        - variables-sensitive.sops.yaml
      """;
    string variablesKustomizationPath = Path.Combine(variablesDirectory, "kustomization.yaml");
    var variablesKustomizationFile = File.Create(variablesKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the variables kustomization.yaml file at {variablesKustomizationPath}.");
    await variablesKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(variablesKustomizationContent));
    await variablesKustomizationFile.FlushAsync();

    Console.WriteLine($"✚ Creating variables file '{clusterDirectory}/variables/variables.yaml'");
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
    string variablesYamlPath = Path.Combine(variablesDirectory, "variables.yaml");
    var variablesYamlFile = File.Create(variablesYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the variables.yaml file at {variablesYamlPath}.");
    await variablesYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesYamlContent));
    await variablesYamlFile.FlushAsync();

    Console.WriteLine($"✚ Creating variables-sensitive file '{clusterDirectory}/variables/variables-sensitive.sops.yaml'");
    const string variablesSensitiveYamlContent = """
      # You need to encrypt this file with SOPS manually.
      # ksail sops --encrypt variables-sensitive.sops.yaml
      apiVersion: v1
      kind: Secret
      metadata:
        name: variables-sensitive
      stringData: {}
      """;
    string variablesSensitiveYamlPath = Path.Combine(variablesDirectory, "variables-sensitive.sops.yaml");
    var variablesSensitiveYamlFile = File.Create(variablesSensitiveYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the variables-sensitive.sops.yaml file at {variablesSensitiveYamlPath}.");
    await variablesSensitiveYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesSensitiveYamlContent));
    await variablesSensitiveYamlFile.FlushAsync();
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
