using KSail.CLIWrappers;

namespace KSail.Provisioners.Cluster;

sealed class K3dProvisioner() : IClusterProvisioner
{
  public async Task ProvisionAsync(string name, string? configPath = null)
  {
    if (!string.IsNullOrEmpty(configPath))
    {
      await K3dCLIWrapper.CreateClusterFromConfigAsync(configPath);
    }
    else
    {
      await K3dCLIWrapper.CreateClusterAsync(name);
    }
  }

  public async Task DeprovisionAsync(string name)
   => await K3dCLIWrapper.DeleteClusterAsync(name);
}
