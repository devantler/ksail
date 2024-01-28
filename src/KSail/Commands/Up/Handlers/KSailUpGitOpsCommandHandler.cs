using KSail.Commands.Check.Handlers;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Update.Handlers;
using KSail.Provisioners;
using KSail.Provisioners.ContainerEngine;

namespace KSail.Commands.Up.Handlers;

class KSailUpCommandHandler(IContainerEngineProvisioner containerEngineProvisioner)
{
  readonly IContainerEngineProvisioner ContainerEngineProvisioner = containerEngineProvisioner;
  internal async Task HandleAsync(string name, string configPath, string manifestsPath, string kustomizationsPath, int timeout, bool noSOPS)
  {
    kustomizationsPath = string.IsNullOrEmpty(kustomizationsPath) ? $"clusters/{name}/flux-system" : kustomizationsPath;

    await ContainerEngineProvisioner.CheckReadyAsync();

    if (await K3dProvisioner.ExistsAsync(name))
    {
      var downHandler = new KSailDownCommandHandler(ContainerEngineProvisioner);
      await downHandler.HandleAsync(name);
    }

    await KSailLintCommandHandler.HandleAsync(name, manifestsPath);

    Console.WriteLine("🧮 Creating pull-through registries...");
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
    await ContainerEngineProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
    Console.WriteLine();
    Console.WriteLine("🧮 Creating OCI registry...");
    await ContainerEngineProvisioner.CreateRegistryAsync("manifests", 5050);
    Console.WriteLine("");
    await KSailUpdateCommandHandler.HandleAsync(name, manifestsPath, true, true);

    await K3dProvisioner.ProvisionAsync(name, configPath);
    var kubernetesProvisioner = new KubernetesProvisioner();
    await kubernetesProvisioner.CreateNamespaceAsync("flux-system");

    if (!noSOPS)
    {
      Console.WriteLine("🔐 Adding SOPS key...");
      var sopsProvisioner = new SOPSProvisioner();
      await sopsProvisioner.ProvisionAsync();
      Console.WriteLine("");
    }

    await FluxProvisioner.CheckPrerequisitesAsync();
    await FluxProvisioner.InstallAsync($"oci://host.k3d.internal:5050/{name}", kustomizationsPath);
    await KSailCheckCommandHandler.HandleAsync(name, timeout, new CancellationToken());
  }
}
