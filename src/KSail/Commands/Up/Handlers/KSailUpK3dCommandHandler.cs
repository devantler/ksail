using KSail.Commands.Down.Handlers;
using KSail.Provisioners.Cluster;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Utils;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dCommandHandler
{
  static readonly K3dProvisioner _clusterProvisioner = new();
  static readonly DockerProvisioner _dockerProvisioner = new();
  internal static async Task HandleAsync(bool shouldPrompt, string name, bool pullThroughRegistries, string configPath)
  {
    if (shouldPrompt)
    {
      bool shouldUseConfig = bool.Parse(ConsoleUtils.Prompt("Use config", "true", RegexFilters.YesNoFilter()));
      if (shouldUseConfig)
      {
        configPath = ConsoleUtils.Prompt("Path to config file", "./k3d-config.yaml", RegexFilters.PathFilter());
      }
      else
      {
        name = ConsoleUtils.Prompt("Name of the cluster");
      }
      pullThroughRegistries = bool.Parse(ConsoleUtils.Prompt("Pull through registries", "true", RegexFilters.YesNoFilter()));
    }
    await _dockerProvisioner.CheckReadyAsync();

    await KSailDownK3dCommandHandler.HandleAsync(name);
    if (pullThroughRegistries)
    {
      Console.WriteLine("🧮 Creating pull-through registries...");
      await _dockerProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      await _dockerProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
      Console.WriteLine();
    }

    await _clusterProvisioner.ProvisionAsync(name, pullThroughRegistries, configPath);
  }
}
