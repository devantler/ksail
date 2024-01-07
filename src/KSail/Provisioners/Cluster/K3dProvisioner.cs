using KSail.CLIWrappers;

namespace KSail.Provisioners.Cluster;

sealed class K3dProvisioner() : IClusterProvisioner
{
  public async Task ProvisionAsync(string name, bool pullThroughRegistries, string? configPath = null)
  {
    Console.WriteLine($"🚀 Provisioning K3d cluster '{name}'...");
    if (!string.IsNullOrEmpty(configPath))
    {
      await K3dCLIWrapper.CreateClusterFromConfigAsync(configPath);
    }
    else
    {
      await K3dCLIWrapper.CreateClusterAsync(name, pullThroughRegistries);
    }
    Console.WriteLine();
  }

  public async Task DeprovisionAsync(string name)
  {
    Console.WriteLine($"🔥 Destroying K3d cluster '{name}'...");
    await K3dCLIWrapper.DeleteClusterAsync(name);
  }

  public async Task ListAsync()
  {
    Console.WriteLine("📋 Listing K3d clusters...");
    await K3dCLIWrapper.ListClustersAsync();
  }
}
