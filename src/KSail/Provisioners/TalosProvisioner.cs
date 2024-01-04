


namespace KSail.Provisioners;

/// <summary>
/// A provisioner for provisioning Talos clusters.
/// </summary>
public class TalosProvisioner : IProvisioner
{
  public Task CreateAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null) => throw new NotImplementedException();
  public Task DestroyAsync(string name) => throw new NotImplementedException();
}
