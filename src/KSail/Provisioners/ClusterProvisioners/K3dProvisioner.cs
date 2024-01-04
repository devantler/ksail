using CliWrap.EventStream;
using KSail.CLIWrappers;

namespace KSail.Provisioners.ClusterProvisioners;

/// <summary>
/// A provisioner for provisioning K3d clusters.
/// </summary>
public class K3dProvisioner() : IClusterProvisioner
{
  /// <inheritdoc/>
  public async Task ProvisionAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null)
  {
    await foreach (var cmdEvent in !string.IsNullOrEmpty(configPath) ? K3dCLIWrapper.K3d.WithArguments($"cluster create --config {configPath}").ListenAsync() : K3dCLIWrapper.K3d.WithArguments($"cluster create {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }

  /// <inheritdoc/>
  public async Task DeprovisionAsync(string name)
  {
    await foreach (var cmdEvent in K3dCLIWrapper.K3d.WithArguments($"cluster delete {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }
}
