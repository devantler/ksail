using KSail.Models.Kubernetes.FluxKustomization;

namespace KSail.Commands.Init.Generators;

class InitFilesGenerator : IDisposable
{
  readonly KubernetesGenerator _kubernetesGenerator = new();
  readonly K3dGenerator _k3dGenerator = new();
  readonly SOPSGenerator _sopsGenerator = new();

  internal async Task GenerateInitFiles(string clusterName, string rootDirectory, CancellationToken token)
  {
    string clusterDirectory = Path.Combine(rootDirectory, "clusters", clusterName);
    string manifestsDirectory = Path.Combine(rootDirectory, "manifests");
    string fluxSystemDirectory = Path.Combine(clusterDirectory, "flux-system");
    await GenerateFluxKustomizations(clusterName, fluxSystemDirectory);
    await GenerateRepositories(manifestsDirectory);
    await GeneratePostBuildVariables(clusterDirectory, clusterName);
    await GenerateInfrastructure(manifestsDirectory);
    await GenerateApps(manifestsDirectory);

    await _k3dGenerator.GenerateK3dConfigAsync($"./{clusterName}-k3d-config.yaml", clusterName);
    //TODO: await GenerateKSailConfigFileAsync($"{clusterName}-ksail-config.yaml");
    await _sopsGenerator.GenerateSOPSConfigAsync(rootDirectory, token);
  }

  async Task GenerateFluxKustomizations(string clusterName, string fluxSystemDirectory)
  {
    await GenerateRepositoriesFluxKustomization(fluxSystemDirectory);
    await GenerateVariablesFluxKustomization(clusterName, fluxSystemDirectory);
    await GenerateInfrastructureFluxKustomization(fluxSystemDirectory);
    await GenerateAppsFluxKustomization(fluxSystemDirectory);
  }

  Task GenerateRepositoriesFluxKustomization(string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "repositories.yaml"),
    [
        new()
          {
            Name = "repositories",
            Path = "manifests/repositories",
            Decryption = null,
            PostBuild = null
          }
    ]);
  }

  Task GenerateVariablesFluxKustomization(string clusterName, string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "variables.yaml"),
    [
        new()
          {
            Name = "variables",
            Path = $"clusters/{clusterName}/variables",
            PostBuild = null,
            DependsOn = ["repositories"]
          }
    ]);
  }

  Task GenerateInfrastructureFluxKustomization(string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "infrastructure.yaml"),
    [
        new FluxKustomizationContent
      {
          Name = "infrastructure",
          Path = "manifests/infrastructure",
          DependsOn = ["variables"]
      }
    ]);
  }

  Task GenerateAppsFluxKustomization(string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "apps.yaml"),
    [
        new() {
          Name = "apps",
          Path = "manifests/apps",
          DependsOn = ["infrastructure"]
      }
    ]);
  }

  Task GenerateRepositories(string manifestsDirectory)
  {
    return _kubernetesGenerator.GenerateOCIRepositoryAsync(Path.Combine(manifestsDirectory, "repositories/oci-artifacts.yaml"),
      new()
      {
        Name = "oci-artifacts",
        Namespace = "flux-system",
        Interval = "1m",
        Url = "oci://ghcr.io/devantler/oci-artifacts/manifests",
        Tag = "latest"
      }
    );
  }

  static async Task GeneratePostBuildVariables(string clusterDirectory, string clusterName)
  {
    await KubernetesGenerator.GenerateConfigMapAsync(Path.Combine(clusterDirectory, "variables/variables.yaml"), clusterName);
    await KubernetesGenerator.GenerateSecretAsync(Path.Combine(clusterDirectory, "variables/variables-sensitive.sops.yaml"));
  }

  async Task GenerateInfrastructure(string manifestsDirectory)
  {
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/kustomization.yaml"),
    [
      "cert-manager",
      "traefik"
    ]);
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/cert-manager/kustomization.yaml"),
    [
      "namespace.yaml",
      "cert-manager.yaml",
      "selfsigned-cluster-issuer.yaml"
    ]);
    await _kubernetesGenerator.GenerateNamespaceAsync(Path.Combine(manifestsDirectory, "infrastructure/cert-manager/namespace.yaml"), "cert-manager");
    await _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/cert-manager/cert-manager.yaml"),
    [
      new FluxKustomizationContent
      {
        Name = "cert-manager",
        Path = "cert-manager",
        SourceRef = new(){
          Kind = "OCIRepository",
          Name = "oci-artifacts"
        }
      }
    ]);
    await _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/cert-manager/selfsigned-cluster-issuer.yaml"),
    [
      new FluxKustomizationContent
      {
        Name = "selfsigned-cluster-issuer",
        Path = "cert-manager/cluster-issuers/selfsigned",
        SourceRef = new(){
          Kind = "OCIRepository",
          Name = "oci-artifacts"
        },
        DependsOn = ["cert-manager"]
      }
    ]);

    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/traefik/kustomization.yaml"),
    [
      "namespace.yaml",
      "traefik.yaml"
    ]);
    await _kubernetesGenerator.GenerateNamespaceAsync(Path.Combine(manifestsDirectory, "infrastructure/traefik/namespace.yaml"), "traefik");
    await _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/traefik/traefik.yaml"),
      [
        new FluxKustomizationContent
        {
          Name = "traefik",
          Path = "traefik",
          SourceRef = new(){
            Kind = "OCIRepository",
            Name = "oci-artifacts"
          }
        }
      ]);
  }

  async Task GenerateApps(string manifestsDirectory)
  {
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "apps/kustomization.yaml"),
        [
          "podinfo"
        ]);
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "apps/podinfo/kustomization.yaml"),
    [
      "namespace.yaml",
      "https://github.com/stefanprodan/podinfo//kustomize"
    ], "podinfo");
    await _kubernetesGenerator.GenerateNamespaceAsync(Path.Combine(manifestsDirectory, "apps/podinfo/namespace.yaml"), "podinfo");
  }

  public void Dispose()
  {
    _sopsGenerator.Dispose();
    GC.SuppressFinalize(this);
  }
}
