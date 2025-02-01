namespace KSail.Models.Project;

/// <summary>
/// The secret manager to use.
/// </summary>
public enum KSailSecretManager
{
  /// <summary>
  /// No secret manager.
  /// </summary>
  None,

  /// <summary>
  /// SOPS secret manager.
  /// </summary>
  SOPS
}
