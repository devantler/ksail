using KSail.CLIWrappers;

namespace KSail.Provisioners;

internal sealed class K3dProvisioner() : IProvisioner
{
  internal static async Task ProvisionAsync(string name, string configPath)
  {
    Console.WriteLine($"🚀 Provisioning K3d cluster '{name}'...");
    await K3dCLIWrapper.CreateClusterAsync(name, configPath);
    Console.WriteLine();
  }

  internal static async Task DeprovisionAsync(string name)
  {
    Console.WriteLine($"🔥 Destroying K3d cluster '{name}'...");
    await K3dCLIWrapper.DeleteClusterAsync(name);
  }

  internal static async Task ListAsync()
  {
    Console.WriteLine("📋 Listing K3d clusters...");
    _ = await K3dCLIWrapper.ListClustersAsync();
  }

  internal static async Task<bool> ExistsAsync(string name) =>
    await K3dCLIWrapper.GetClusterAsync(name);
}
