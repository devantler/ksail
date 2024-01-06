namespace KSail.Provisioners.GitOps;

/// <summary>
/// A provisioner for provisioning GitOps.
/// </summary>
public interface IGitOpsProvisioner : IProvisioner
{
  /// <summary>
  /// Check for prerequisites for installing GitOps.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task CheckPrerequisitesAsync();

  /// <summary>
  /// Installs GitOps.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task InstallAsync();

  /// <summary>
  /// Uninstalls GitOps.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task UninstallAsync();
}
