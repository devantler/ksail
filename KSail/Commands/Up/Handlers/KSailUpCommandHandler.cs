using KSail.Commands.Check.Handlers;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;
using KSail.Provisioners.SecretManager;

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
  internal async Task<int> HandleAsync(string clusterName, string configPath, string manifestsPath, string kustomizationsPath, int timeout, bool noSOPS, bool skipLinting, CancellationToken token)
  {
    kustomizationsPath = string.IsNullOrEmpty(kustomizationsPath) ? $"clusters/{clusterName}/flux-system" : kustomizationsPath;

    Console.WriteLine("🐳 Checking Docker is running");
    if (await _containerEngineProvisioner.CheckReadyAsync(token).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    Console.WriteLine("✔ Docker is running");
    Console.WriteLine("");

    var (exitCode, result) = await _kubernetesDistributionProvisioner.ExistsAsync(clusterName, token).ConfigureAwait(false);
    if (exitCode != 0)
    {
      return 1;
    }
    if (result)
    {
      var downHandler = new KSailDownCommandHandler(_containerEngineProvisioner, _kubernetesDistributionProvisioner);
      if (await downHandler.HandleAsync(clusterName, token).ConfigureAwait(false) != 0)
      {
        return 1;
      }
    }

    if (!skipLinting && await KSailLintCommandHandler.HandleAsync(clusterName, manifestsPath, token).ConfigureAwait(false) != 0)
    {
      return 1;
    }

    Console.WriteLine("🧮 Creating pull-through registries");
    if (await _containerEngineProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, token, new Uri("https://registry-1.docker.io")).ConfigureAwait(false) != 0 ||
      await _containerEngineProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, token, new Uri("https://registry.k8s.io")).ConfigureAwait(false) != 0 ||
      await _containerEngineProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, token, new Uri("https://gcr.io")).ConfigureAwait(false) != 0 ||
      await _containerEngineProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, token, new Uri("https://ghcr.io")).ConfigureAwait(false) != 0 ||
      await _containerEngineProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, token, new Uri("https://quay.io")).ConfigureAwait(false) != 0 ||
      await _containerEngineProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, token, new Uri("https://mcr.microsoft.com")).ConfigureAwait(false) != 0
    )
    {
      return 1;
    }
    Console.WriteLine();

    Console.WriteLine("🧮 Creating OCI registry");
    if (await _containerEngineProvisioner.CreateRegistryAsync("manifests", 5050, token).ConfigureAwait(false) != 0)
    {
      return 1;
    }
    Console.WriteLine("");

    if (await new KSailUpdateCommandHandler(_kubernetesDistributionProvisioner, _gitOpsProvisioner).HandleAsync(clusterName, manifestsPath, true, true, token).ConfigureAwait(false) != 0)
    {
      return 1;
    }

    Console.WriteLine($"🚀 Provisioning cluster '{clusterName}'");
    if (await _kubernetesDistributionProvisioner.ProvisionAsync(clusterName, configPath, token).ConfigureAwait(false) != 0)
    {
      Console.WriteLine($"✕ Failed to provision cluster '{clusterName}'");
      return 1;
    }
    Console.WriteLine("");

    Console.WriteLine("🌐 Creating 'flux-system' namespace");
    var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync().ConfigureAwait(false);
#pragma warning disable CA1308 // Normalize strings to uppercase
    string context = $"{kubernetesDistributionType.ToString()?.ToLowerInvariant()}-{clusterName}";
#pragma warning restore CA1308 // Normalize strings to uppercase
    await _containerOrchestratorProvisioner.CreateNamespaceAsync(context, "flux-system").ConfigureAwait(false);
    Console.WriteLine("");

    if (!noSOPS)
    {
      Console.WriteLine("🔐 Adding SOPS key");
      using var sopsProvisioner = new LocalSOPSProvisioner();
      Console.WriteLine("► Provisioning key for SOPS");
      if (await sopsProvisioner.ProvisionAsync(KeyType.Age, clusterName, context, token).ConfigureAwait(false) != 0)
      {
        Console.WriteLine("✕ SOPS key provisioning failed");
        return 1;
      }
      Console.WriteLine("");
    }
    var kubernetesDistribution = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync().ConfigureAwait(false);
#pragma warning disable CA1308 // Normalize strings to uppercase
    string k8sContext = $"{kubernetesDistribution.ToString()?.ToLowerInvariant()}-{clusterName}";
#pragma warning restore CA1308 // Normalize strings to uppercase
    string ociUrl = $"oci://host.k3d.internal:5050/{clusterName}";

    Console.WriteLine("🔼 Installing Flux");
    if (await _gitOpsProvisioner.InstallAsync(k8sContext, ociUrl, kustomizationsPath, token).ConfigureAwait(false) != 0)
    {
      Console.WriteLine("✕ Failed to install Flux");
      return 1;
    }
    Console.WriteLine("");

    Console.WriteLine("🔄 Checking Flux reconciles successfully");
    return await new KSailCheckCommandHandler().HandleAsync(context, timeout, token).ConfigureAwait(false) != 0 ? 1 : 0;
  }
}
