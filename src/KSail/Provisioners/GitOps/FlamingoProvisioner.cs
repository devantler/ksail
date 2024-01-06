
namespace KSail.Provisioners.GitOps;

/// <summary>
/// A provisioner for provisioning Flamigo.
/// </summary>
public class FlamingoProvisioner : IGitOpsProvisioner
{
  /// <inheritdoc/>
  public Task CheckPrerequisitesAsync() => throw new NotImplementedException();

  /// <inheritdoc/>
  public Task InstallAsync() => throw new NotImplementedException();

  /// <inheritdoc/>
  public Task UninstallAsync() => throw new NotImplementedException();
}
