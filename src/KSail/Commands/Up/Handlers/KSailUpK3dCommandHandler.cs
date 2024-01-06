using KSail.Commands.Down.Handlers;
using KSail.Provisioners.Cluster;
using KSail.Provisioners.Registry;
using KSail.Utils;

namespace KSail.Commands.Up.Handlers;

/// <summary>
/// The command handler responsible for handling the <c>ksail up k3d</c> command.
/// </summary>
public static class KSailUpK3dCommandHandler
{
  static readonly K3dProvisioner _clusterProvisioner = new();
  static readonly DockerRegistryProvisioner _registryProvisioner = new();

  /// <summary>
  /// Handles the <c>ksail up k3d</c> command.
  /// </summary>
  /// <param name="name">The name of the cluster.</param>
  /// <param name="pullThroughRegistries">Whether to create pull-through registries.</param>
  /// <param name="configPath">The path to the cluster configuration file.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public static async Task Handle(string name, bool pullThroughRegistries, string configPath)
  {
    bool shouldPrompt = string.IsNullOrEmpty(name) && string.IsNullOrEmpty(configPath);
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
      Console.WriteLine("🧮 Creating pull-through registries...");
      await _registryProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await _registryProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      //TODO: Add missing major registries
    }
    Console.WriteLine();
    Console.WriteLine($"🚀 Provisioning K3d cluster '{name}'...");
    await _clusterProvisioner.ProvisionAsync(name, configPath);
  }
}
