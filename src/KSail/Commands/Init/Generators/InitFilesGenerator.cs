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
    await GeneratePostBuildVariables(clusterDirectory);
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
  }

  Task GenerateAppsFluxKustomization(string fluxSystemDirectory)
  {
    return _kubernetesGenerator.GenerateFluxKustomizationAsync(Path.Combine(fluxSystemDirectory, "apps.yaml"),
    [
        new() {
          Name = "apps",
          Path = "apps",
          DependsOn = ["infrastructure-configs"]
      }
    ]);
  }

  async Task GeneratePostBuildVariables(string clusterDirectory)
  {
    await _kubernetesGenerator.GenerateKustomizationAsync(
      Path.Combine(clusterDirectory, "variables/kustomization.yaml"),
      ["variables.yaml", "variables-sensitive.sops.yaml"],
      "flux-system"
    );
    await KubernetesGenerator.GenerateConfigMapAsync(Path.Combine(clusterDirectory, "variables/variables.yaml"));
    await KubernetesGenerator.GenerateSecretAsync(Path.Combine(clusterDirectory, "variables/variables-sensitive.sops.yaml"));
  }

  async Task GenerateInfrastructure(string manifestsDirectory)
  {
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/services/kustomization.yaml"),
        [
          "https://github.com/devantler/oci-artifacts//k8s/cert-manager",
          "https://github.com/devantler/oci-artifacts//k8s/traefik"
        ]);
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/configs/kustomization.yaml"),
    [
      "https://raw.githubusercontent.com/devantler/oci-artifacts/main/k8s/cert-manager/cluster-issuers/selfsigned/cluster-issuer-certificate.yaml",
      "https://raw.githubusercontent.com/devantler/oci-artifacts/main/k8s/cert-manager/cluster-issuers/selfsigned/selfsigned-cluster-issuer.yaml"
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
