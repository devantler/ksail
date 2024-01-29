using System.Globalization;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Provisioners;
using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler(
  IContainerEngineProvisioner containerEngineProvisioner,
  IKubernetesDistributionProvisioner kubernetesDistributionProvisioner,
  IContainerOrchestratorProvisioner containerOrchestratorProvisioner,
  IGitOpsProvisioner gitOpsProvisioner
)
{
  readonly IContainerEngineProvisioner _containerEngineProvisioner = containerEngineProvisioner;
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;
  readonly IContainerOrchestratorProvisioner _containerOrchestratorProvisioner = containerOrchestratorProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;
  internal async Task<int> HandleAsync(string clusterName, string configPath, string manifestsPath, string kustomizationsPath, int timeout, bool noSOPS, CancellationToken token)
  {
    kustomizationsPath = string.IsNullOrEmpty(kustomizationsPath) ? $"clusters/{clusterName}/flux-system" : kustomizationsPath;

    await _containerEngineProvisioner.CheckReadyAsync();

    var (ExitCode, Result) = await _kubernetesDistributionProvisioner.ExistsAsync(clusterName, token);
    if (ExitCode != 0)
    {
      return 1;
    }
    if (Result)
    {
      var downHandler = new KSailDownCommandHandler(_containerEngineProvisioner, _kubernetesDistributionProvisioner);
      if (await downHandler.HandleAsync(clusterName, token) != 0)
      {
        return 1;
      }
    }

    if (await KSailLintCommandHandler.HandleAsync(clusterName, manifestsPath, token) != 0)
    {
      return 1;
    }

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

    if (await new KSailUpdateCommandHandler(_kubernetesDistributionProvisioner, _gitOpsProvisioner).HandleAsync(clusterName, manifestsPath, true, true, token) != 0)
    {
      return 1;
    }

    if (await _kubernetesDistributionProvisioner.ProvisionAsync(clusterName, configPath, token) != 0)
    {
      return 1;
    }
    var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
    string context = $"{kubernetesDistributionType.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";
    await _containerOrchestratorProvisioner.CreateNamespaceAsync(context, "flux-system");

    if (!noSOPS)
    {
      Console.WriteLine("🔐 Adding SOPS key...");
      var sopsProvisioner = new SOPSProvisioner();
      await sopsProvisioner.ProvisionAsync(context);
      Console.WriteLine("");
    }
    var kubernetesDistribution = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
    string k8sContext = $"{kubernetesDistribution.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";
    string ociUrl = $"oci://host.k3d.internal:5050/{clusterName}";
    return await _gitOpsProvisioner.InstallAsync(k8sContext, ociUrl, kustomizationsPath, token) switch
    {
      0 => await new KSailCheckCommandHandler().HandleAsync(context, timeout, token) != 0 ? 1 : 0,
      _ => 1,
    };
  }
}
