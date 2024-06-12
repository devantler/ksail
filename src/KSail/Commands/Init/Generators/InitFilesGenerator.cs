using KSail.Models.Kubernetes.FluxKustomization;

namespace KSail.Commands.Init.Generators;

class InitFilesGenerator : IDisposable
{
  readonly KubernetesGenerator _kubernetesGenerator = new();
  readonly K3dGenerator _k3dGenerator = new();
  readonly SOPSGenerator _sopsGenerator = new();

  internal async Task GenerateInitFiles(string clusterName, string manifestsDirectory, CancellationToken token)
  {
    string clusterDirectory = Path.Combine(manifestsDirectory, "clusters", clusterName);
    string fluxSystemDirectory = Path.Combine(clusterDirectory, "flux-system");
    await GenerateFluxKustomizations(clusterName, fluxSystemDirectory);
    await GeneratePostBuildVariables(clusterDirectory, clusterName);
    await GenerateInfrastructure(manifestsDirectory);
    await GenerateApps(manifestsDirectory);

    await _k3dGenerator.GenerateK3dConfigAsync($"./{clusterName}-k3d-config.yaml", clusterName);
    //TODO: await GenerateKSailConfigFileAsync($"{clusterName}-ksail-config.yaml");
    await _sopsGenerator.GenerateSOPSConfigAsync(manifestsDirectory, token);
  }

  async Task GenerateFluxKustomizations(string clusterName, string fluxSystemDirectory)
  {
    await GenerateVariablesFluxKustomization(clusterName, fluxSystemDirectory);
    await GenerateInfrastructureFluxKustomization(fluxSystemDirectory);
    await GenerateAppsFluxKustomization(fluxSystemDirectory);
  }

  Task GenerateVariablesFluxKustomization(string clusterName, string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "variables.yaml"),
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
  }

  Task GenerateInfrastructureFluxKustomization(string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "infrastructure.yaml"),
    [
        new FluxKustomizationContent
      {
          Name = "infrastructure",
          Path = "infrastructure",
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
          Path = "apps",
          DependsOn = ["infrastructure"]
      }
    ]);
  }

  async Task GeneratePostBuildVariables(string clusterDirectory, string clusterName)
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
        Path = "cert-manager"
      }
    ]);
    await _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/cert-manager/selfsigned-cluster-issuer.yaml"),
    [
      new FluxKustomizationContent
      {
        Name = "selfsigned-cluster-issuer",
        Path = "cert-manager/cluster-issuers/selfsigned",
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
          Path = "traefik"
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
