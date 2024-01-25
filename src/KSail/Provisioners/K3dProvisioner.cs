using KSail.CLIWrappers;

namespace KSail.Provisioners;

sealed class K3dProvisioner() : IProvisioner
{
  internal static async Task ProvisionAsync(string name, string configPath)
  {
    Console.WriteLine($"🚀 Provisioning K3d cluster '{name}'...");
    await K3dCLIWrapper.CreateClusterAsync(name, configPath);
    Console.WriteLine();
  }

  internal static Task DeprovisionAsync(string name)
  {
    Console.WriteLine($"🔥 Destroying K3d cluster '{name}'...");
    return K3dCLIWrapper.DeleteClusterAsync(name);
  }

  internal static async Task<string> ListAsync() => _ = await K3dCLIWrapper.ListClustersAsync();

  internal static Task<bool> ExistsAsync(string name) => K3dCLIWrapper.GetClusterAsync(name);
}
