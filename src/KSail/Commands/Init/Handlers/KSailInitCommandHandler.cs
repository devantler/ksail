using System.Text;
using KSail.Provisioners;

namespace KSail.Commands.Init.Handlers;

static class KSailInitCommandHandler
{
  internal static async Task<int> HandleAsync(string clusterName, string manifests, CancellationToken token)
  {
    Console.WriteLine($"📁 Initializing a new K8s GitOps project named '{clusterName}'...");
    string clusterDirectory = Path.Combine(manifests, "clusters", clusterName);
    if (Directory.Exists(clusterDirectory))
    {
      Console.WriteLine($"✕ A cluster named '{clusterName}' already exists at '{clusterDirectory}/{clusterName}'. Skipping cluster creation.");
    }
    else
    {
      clusterDirectory = CreateClusterDirectory(clusterName, manifests);
      await CreateFluxKustomizationsAsync(clusterName, clusterDirectory);
      await CreateKustomizationsAsync(clusterDirectory);
    }
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), $"{clusterName}-k3d-config.yaml")))
    {
      Console.WriteLine($"✕ A k3d-config.yaml file already exists at '{Directory.GetCurrentDirectory()}/{clusterName}-k3d-config.yaml'. Skipping config creation.");
    }
    else
    {
      await CreateConfigAsync(clusterName);
    }
    if (await SOPSProvisioner.CreateKeysAsync(token) != 0)
    {
      return 1;
    }

    await SOPSProvisioner.CreateSOPSConfigAsync($"{manifests}/../.sops.yaml");
    Console.WriteLine($"✔ Successfully initialized a new K8s GitOps project named '{clusterName}'.");
    Console.WriteLine();
    return 0;
  }

  static string CreateFluxSystemDirectory(string clusterDirectory)
  {
    Console.WriteLine($"✚ Creating flux-system directory '{clusterDirectory}/flux-system'...");
    string fluxDirectory = Path.Combine(clusterDirectory, "flux-system");
    _ = Directory.CreateDirectory(fluxDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the flux directory at {fluxDirectory}.");
    return fluxDirectory;
  }

  static string CreateClusterDirectory(string clusterName, string manifests)
  {
    string clusterDirectory = Path.Combine(manifests, "clusters", clusterName);
    Console.WriteLine($"✚ Creating cluster directory '{clusterDirectory}'...");
    _ = Directory.CreateDirectory(clusterDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the cluster directory at {clusterDirectory}.");
    return clusterDirectory;
  }

  static async Task CreateFluxKustomizationsAsync(string clusterName, string clusterDirectory)
  {
    Console.WriteLine($"✚ Creating flux infrastructure kustomization '{clusterDirectory}/flux-system/infrastructure.yaml'...");
    string fluxDirectory = CreateFluxSystemDirectory(clusterDirectory);
    string infrastructureYamlPath = Path.Combine(fluxDirectory, "infrastructure.yaml");
    string infrastructureYamlContent = $"""
      apiVersion: kustomize.toolkit.fluxcd.io/v1
      kind: Kustomization
      metadata:
        name: infrastructure-services
        namespace: flux-system
      spec:
        interval: 1m
        dependsOn:
          - name: variables
        sourceRef:
          kind: OCIRepository
          name: flux-system
        path: ./clusters/{clusterName}/infrastructure/services
        prune: true
        wait: true
        decryption:
          provider: sops
          secretRef:
            name: sops-age
        postBuild:
          substituteFrom:
            - kind: ConfigMap
              name: variables
            - kind: Secret
              name: variables-sensitive
      ---
      apiVersion: kustomize.toolkit.fluxcd.io/v1
      kind: Kustomization
      metadata:
        name: infrastructure-configs
        namespace: flux-system
      spec:
        interval: 1m
        dependsOn:
          - name: variables
          - name: infrastructure-services
        sourceRef:
          kind: OCIRepository
          name: flux-system
        path: ./clusters/{clusterName}/infrastructure/configs
        prune: true
        wait: true
        decryption:
          provider: sops
          secretRef:
            name: sops-age
        postBuild:
          substituteFrom:
            - kind: ConfigMap
              name: variables
            - kind: Secret
              name: variables-sensitive
      """;
    var infrastructureYamlFile = File.Create(infrastructureYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure.yaml file at {infrastructureYamlPath}.");
    await infrastructureYamlFile.WriteAsync(Encoding.UTF8.GetBytes(infrastructureYamlContent));
    await infrastructureYamlFile.FlushAsync();

    Console.WriteLine($"✚ Creating flux variables kustomization '{clusterDirectory}/flux-system/variables.yaml'...");
    string variablesYamlContent = $"""
      apiVersion: kustomize.toolkit.fluxcd.io/v1
      kind: Kustomization
      metadata:
        name: variables
        namespace: flux-system
      spec:
        interval: 1m
        sourceRef:
          kind: OCIRepository
          name: flux-system
        path: ./clusters/{clusterName}/variables
        prune: true
        wait: true
        decryption:
          provider: sops
          secretRef:
            name: sops-age
    """;
    string variablesYamlPath = Path.Combine(fluxDirectory, "variables.yaml");
    var variablesYamlFile = File.Create(variablesYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the variables.yaml file at {variablesYamlPath}.");
    await variablesYamlFile.WriteAsync(Encoding.UTF8.GetBytes(variablesYamlContent));
    await variablesYamlFile.FlushAsync();
  }

  static async Task CreateKustomizationsAsync(string clusterDirectory)
  {
    Console.WriteLine($"✚ Creating infrastructure-services kustomization '{clusterDirectory}/infrastructure/services/kustomization.yaml'...");
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

    Console.WriteLine($"✚ Creating infrastructure-configs kustomization '{clusterDirectory}/infrastructure/configs/kustomization.yaml'...");
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

    Console.WriteLine($"✚ Creating variables kustomization '{clusterDirectory}/variables/kustomization.yaml'...");
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

    Console.WriteLine($"✚ Creating variables file '{clusterDirectory}/variables/variables.yaml'...");
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

    Console.WriteLine($"✚ Creating variables-sensitive file '{clusterDirectory}/variables/variables-sensitive.sops.yaml'...");
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
    Console.WriteLine($"✚ Creating config file './{clusterName}-k3d-config.yaml'...");
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
}
