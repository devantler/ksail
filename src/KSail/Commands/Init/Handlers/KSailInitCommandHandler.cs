using System.Text;

namespace KSail.Commands.Init.Handlers;

static class KSailInitCommandHandler
{
  internal static Task HandleAsync(string name, string manifests)
  {
    Console.WriteLine($"📁 Initializing a new K8s GitOps project named '{name}'...");
    string clusterDirectory = CreateClusterDirectory(name, manifests);

    if (Directory.Exists(Path.Combine(clusterDirectory, name)))
    {
      Console.WriteLine($"✕ A cluster named '{name}' already exists at '{clusterDirectory}/{name}'. Skipping cluster creation.");
    }
    else
    {
      CreateFluxKustomizations(name, clusterDirectory);
      CreateKustomizations(clusterDirectory);
    }
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "k3d-config.yaml")))
    {
      Console.WriteLine($"✕ A k3d-config.yaml file already exists at '{Directory.GetCurrentDirectory()}/k3d-config.yaml'. Skipping config creation.");
    }
    else
    {
      CreateConfig(name);
    }
    return Task.CompletedTask;
  }

  static string CreateFluxDirectory(string clusterDirectory)
  {
    string fluxDirectory = Path.Combine(clusterDirectory, "flux-system");
    _ = Directory.CreateDirectory(fluxDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the flux directory at {fluxDirectory}.");
    return fluxDirectory;
  }

  static string CreateClusterDirectory(string name, string manifests)
  {
    //Create the manifests directory if it doesn't exist
    _ = Directory.CreateDirectory(manifests) ?? throw new InvalidOperationException($"🚨 Could not create the manifests directory at {manifests}.");

    //Create a clusters directory in the manifests directory
    string clustersDirectory = Path.Combine(manifests, "clusters");
    _ = Directory.CreateDirectory(clustersDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the clusters directory at {clustersDirectory}.");

    //Create a cluster directory named after the name property in the clusters directory
    string clusterDirectory = Path.Combine(clustersDirectory, name);
    _ = Directory.CreateDirectory(clusterDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the cluster directory at {clusterDirectory}.");
    return clusterDirectory;
  }

  static async void CreateFluxKustomizations(string name, string clusterDirectory)
  {
    string fluxDirectory = CreateFluxDirectory(clusterDirectory);
    string infrastructureYamlPath = Path.Combine(fluxDirectory, "infrastructure.yaml");
    string infrastructureYamlContent = $"""
      apiVersion: kustomize.toolkit.fluxcd.io/v1
      kind: Kustomization
      metadata:
        name: infrastructure
        namespace: flux-system
      spec:
        interval: 1m
        sourceRef:
          kind: OCIRepository
          name: flux-system
        path: ./clusters/{name}/infrastructure
        prune: true
        wait: true
        decryption:
          provider: sops
          secretRef:
            name: sops-age
      """;
    var infrastructureYamlFile = File.Create(infrastructureYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure.yaml file at {infrastructureYamlPath}.");
    await infrastructureYamlFile.WriteAsync(Encoding.UTF8.GetBytes(infrastructureYamlContent));
    await infrastructureYamlFile.FlushAsync();

    string appsYamlContent = $"""
      apiVersion: kustomize.toolkit.fluxcd.io/v1
      kind: Kustomization
      metadata:
        name: apps
        namespace: flux-system
      spec:
        interval: 1m
        sourceRef:
          kind: OCIRepository
          name: flux-system
        path: ./clusters/{name}/apps
        prune: true
        wait: true
        decryption:
          provider: sops
          secretRef:
            name: sops-age
    """;
    string appsYamlPath = Path.Combine(fluxDirectory, "apps.yaml");
    var appsYamlFile = File.Create(appsYamlPath) ?? throw new InvalidOperationException($"🚨 Could not create the apps.yaml file at {appsYamlPath}.");
    await appsYamlFile.WriteAsync(Encoding.UTF8.GetBytes(appsYamlContent));
    await appsYamlFile.FlushAsync();

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
        path: ./clusters/{name}/variables
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

  static async void CreateKustomizations(string clusterDirectory)
  {
    string infrastructureDirectory = Path.Combine(clusterDirectory, "infrastructure");
    _ = Directory.CreateDirectory(infrastructureDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure directory at {infrastructureDirectory}.");
    const string infrastructureKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      resources: []
      """;
    string infrastructureKustomizationPath = Path.Combine(infrastructureDirectory, "kustomization.yaml");
    var infrastructureKustomizationFile = File.Create(infrastructureKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the infrastructure kustomization.yaml file at {infrastructureKustomizationPath}.");
    await infrastructureKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(infrastructureKustomizationContent));
    await infrastructureKustomizationFile.FlushAsync();

    string appsDirectory = Path.Combine(clusterDirectory, "apps");
    _ = Directory.CreateDirectory(appsDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the apps directory at {appsDirectory}.");
    const string appsKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      resources: []
      """;
    string appsKustomizationPath = Path.Combine(appsDirectory, "kustomization.yaml");
    var appsKustomizationFile = File.Create(appsKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the apps kustomization.yaml file at {appsKustomizationPath}.");
    await appsKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(appsKustomizationContent));
    await appsKustomizationFile.FlushAsync();

    string variablesDirectory = Path.Combine(clusterDirectory, "variables");
    _ = Directory.CreateDirectory(variablesDirectory) ?? throw new InvalidOperationException($"🚨 Could not create the variables directory at {variablesDirectory}.");
    const string variablesKustomizationContent = """
      apiVersion: kustomize.config.k8s.io/v1beta1
      kind: Kustomization
      resources: []
      """;
    string variablesKustomizationPath = Path.Combine(variablesDirectory, "kustomization.yaml");
    var variablesKustomizationFile = File.Create(variablesKustomizationPath) ?? throw new InvalidOperationException($"🚨 Could not create the variables kustomization.yaml file at {variablesKustomizationPath}.");
    await variablesKustomizationFile.WriteAsync(Encoding.UTF8.GetBytes(variablesKustomizationContent));
    await variablesKustomizationFile.FlushAsync();
  }

  static async void CreateConfig(string name)
  {
    string configPath = Path.Combine(Directory.GetCurrentDirectory(), "k3d-config.yaml");
    string configContent = $"""
      apiVersion: k3d.io/v1alpha5
      kind: Simple
      metadata:
        name: {name}
      volumes:
        - volume: k3d-{name}-storage:/var/lib/rancher/k3s/storage
      network: k3d-{name}
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
