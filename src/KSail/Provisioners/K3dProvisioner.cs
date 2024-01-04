


using CliWrap.Buffered;
using CliWrap.EventStream;
using KSail.CLIWrappers;

namespace KSail.Provisioners;

/// <summary>
/// A provisioner for provisioning K3d clusters.
/// </summary>
public class K3dProvisioner() : IProvisioner
{



  /// <inheritdoc/>
  public async Task CreateAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null)
  {
    await foreach (var cmdEvent in !string.IsNullOrEmpty(configPath) ? K3dCLIWrapper.K3d.WithArguments($"cluster create --config {configPath}").ListenAsync() : K3dCLIWrapper.K3d.WithArguments($"cluster create {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }

  /// <inheritdoc/>
  public async Task DestroyAsync(string name)
  {
    await foreach (var cmdEvent in K3dCLIWrapper.K3d.WithArguments($"cluster delete {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }
}
