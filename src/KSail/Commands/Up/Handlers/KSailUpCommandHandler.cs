using System.Globalization;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Services.Provisioners;
using KSail.Services.Provisioners.ContainerEngine;
using KSail.Services.Provisioners.ContainerOrchestrator;
using KSail.Services.Provisioners.GitOps;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler(
  IContainerEngineProvisioner containerEngineProvisioner,
  IKubernetesDistributionProvisioner kubernetesDistributionProvisioner,
  IGitOpsProvisioner gitOpsProvisioner
)
{
  readonly IContainerEngineProvisioner _containerEngineProvisioner = containerEngineProvisioner;
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;
  internal async Task HandleAsync(string clusterName, string configPath, string manifestsPath, string kustomizationsPath, int timeout, bool noSOPS)
  {
    kustomizationsPath = string.IsNullOrEmpty(kustomizationsPath) ? $"clusters/{clusterName}/flux-system" : kustomizationsPath;

    await _containerEngineProvisioner.CheckReadyAsync();

    if (await _kubernetesDistributionProvisioner.ExistsAsync(clusterName))
    {
      var downHandler = new KSailDownCommandHandler(_containerEngineProvisioner, _kubernetesDistributionProvisioner);
      await downHandler.HandleAsync(clusterName);
    }

    await KSailLintCommandHandler.HandleAsync(clusterName, manifestsPath);

    Console.WriteLine("🧮 Creating pull-through registries...");
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
    await _containerEngineProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
    Console.WriteLine();
    Console.WriteLine("🧮 Creating OCI registry...");
    await _containerEngineProvisioner.CreateRegistryAsync("manifests", 5050);
    Console.WriteLine("");
    await new KSailUpdateCommandHandler(_kubernetesDistributionProvisioner, _gitOpsProvisioner).HandleAsync(clusterName, manifestsPath, true, true);

    await _kubernetesDistributionProvisioner.ProvisionAsync(clusterName, configPath);
    var kubernetesProvisioner = new KubernetesProvisioner();
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (!noSOPS)
    {
      Console.WriteLine("🔐 Adding SOPS key...");
      var sopsProvisioner = new SOPSProvisioner();
      await sopsProvisioner.ProvisionAsync();
      Console.WriteLine("");
    }
    var kubernetesDistribution = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
    await _gitOpsProvisioner.InstallAsync($"{kubernetesDistribution.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}", $"oci://host.k3d.internal:5050/{clusterName}", kustomizationsPath);
    await KSailCheckCommandHandler.HandleAsync(clusterName, timeout, new CancellationToken());
  }
}
