using KSail.Commands.Init.Generators;
using KSail.Models.Kubernetes.FluxKustomization;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(string clusterName, string manifestsDirectory) : IDisposable
{
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();
  readonly KubernetesGenerator _kubernetesGenerator = new();
  readonly K3dGenerator _k3dGenerator = new();
  readonly SOPSGenerator _sopsGenerator = new();

  internal async Task<int> HandleAsync(CancellationToken token)
  {
    Console.WriteLine($"📁 Initializing new cluster '{clusterName}'");
    if (await ProvisionSOPSKey(clusterName, token) != 0)
    {
      return 1;
    }
    await InitCluster(clusterName, manifestsDirectory, token);
    Console.WriteLine("");
    return 0;
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

  async Task InitCluster(string clusterName, string manifestsDirectory, CancellationToken token)
  {
    string clusterDirectory = Path.Combine(manifestsDirectory, "clusters", clusterName);
    string fluxSystemDirectory = Path.Combine(clusterDirectory, "flux-system");
    await GenerateVariablesFluxKustomization(clusterName, fluxSystemDirectory);
    await GenerateInfrastructureFluxKustomization(fluxSystemDirectory);
    await GenerateAppsFluxKustomization(fluxSystemDirectory);
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(clusterDirectory, "variables/kustomization.yaml"), ["variables.yaml", "variables-sensitive.sops.yaml"], "flux-system");
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/services/kustomization.yaml"),
    [
      "https://github.com/devantler/oci-registry//k8s/cert-manager",
      "https://github.com/devantler/oci-registry//k8s/traefik"
    ]);
    await _kubernetesGenerator.GenerateKustomizationAsync(Path.Combine(manifestsDirectory, "infrastructure/configs/kustomization.yaml"),
    [
      "https://raw.githubusercontent.com/devantler/oci-registry/main/k8s/cert-manager/certificates/cluster-issuer-certificate.yaml",
      "https://raw.githubusercontent.com/devantler/oci-registry/main/k8s/cert-manager/cluster-issuers/selfsigned-cluster-issuer.yaml"
    ]);
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
    await KubernetesGenerator.GenerateConfigMapAsync(Path.Combine(clusterDirectory, "variables/variables.yaml"));
    await KubernetesGenerator.GenerateSecretAsync(Path.Combine(clusterDirectory, "variables/variables-sensitive.sops.yaml"));
    await _k3dGenerator.GenerateK3dConfigAsync($"./{clusterName}-k3d-config.yaml", clusterName);
    //TODO: await GenerateKSailConfigFileAsync($"{clusterName}-ksail-config.yaml");
    await _sopsGenerator.GenerateSOPSConfigAsync(manifestsDirectory, token);
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

  public void Dispose()
  {
    _localSOPSProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
