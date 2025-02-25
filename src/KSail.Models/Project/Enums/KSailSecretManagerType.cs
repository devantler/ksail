namespace KSail.Models.Project.Enums;

/// <summary>
/// The secret manager to use.
/// </summary>
public enum KSailSecretManagerType
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
