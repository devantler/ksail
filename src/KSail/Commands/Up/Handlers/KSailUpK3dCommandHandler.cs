using KSail.Commands.Down.Handlers;
using KSail.Provisioners.Cluster;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Utils;

namespace KSail.Commands.Up.Handlers;

static class KSailUpK3dCommandHandler
{
  static readonly K3dProvisioner _clusterProvisioner = new();
  static readonly DockerProvisioner _registryProvisioner = new();

  internal static async Task Handle(bool shouldPrompt, string name, bool pullThroughRegistries, string configPath)
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
    await KSailDownK3dCommandHandler.Handle(name);
    if (pullThroughRegistries)
    {
      Console.WriteLine();
      await _registryProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      //TODO: Add missing major registries
    }
    Console.WriteLine();

    await _clusterProvisioner.ProvisionAsync(name, pullThroughRegistries, configPath);
  }
}
