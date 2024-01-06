using KSail.CLIWrappers;
using KSail.Provisioners.GitOps;

namespace KSail.Provisioners.Cluster;

/// <summary>
/// A provisioner for provisioning K3d clusters.
/// </summary>
/// <param name="gitOpsProvisioner">The GitOps provisioner.</param>
public class K3dProvisioner(IGitOpsProvisioner gitOpsProvisioner) : IClusterProvisioner
{
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;

  /// <inheritdoc/>
  public async Task ProvisionAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null)
  {
    if (!string.IsNullOrEmpty(configPath))
    {
      await K3dCLIWrapper.CreateClusterFromConfigAsync(configPath);
    }
    else
    {
      await K3dCLIWrapper.CreateClusterAsync(name);
    }
    await _gitOpsProvisioner.CheckPrerequisitesAsync();
    await _gitOpsProvisioner.InstallAsync();
  }

  /// <inheritdoc/>
  public async Task DeprovisionAsync(string name)
    => await K3dCLIWrapper.DeleteClusterAsync(name);
}
