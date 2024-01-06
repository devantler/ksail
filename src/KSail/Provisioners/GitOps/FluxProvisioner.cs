
namespace KSail.Provisioners.GitOps;

/// <summary>
/// A provisioner for provisioning Flux.
/// </summary>
public class FluxProvisioner : IGitOpsProvisioner
{
  /// <inheritdoc/>
  public Task CheckPrerequisitesAsync() => throw new NotImplementedException();

  /// <inheritdoc/>
  public Task InstallAsync() => throw new NotImplementedException();

  /// <inheritdoc/>
  public Task UninstallAsync() => throw new NotImplementedException();
}
