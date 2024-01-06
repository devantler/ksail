using KSail.CLIWrappers;

namespace KSail.Provisioners.Cluster;

/// <summary>
/// A provisioner for provisioning K3d clusters.
/// </summary>
public class K3dProvisioner() : IClusterProvisioner
{
  /// <inheritdoc/>
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

  /// <inheritdoc/>
  public async Task DeprovisionAsync(string name)
    => await K3dCLIWrapper.DeleteClusterAsync(name);
}
