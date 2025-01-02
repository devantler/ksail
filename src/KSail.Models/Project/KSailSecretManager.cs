using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The secret manager to use.
/// </summary>
public enum KSailSecretManager
{
  /// <summary>
  /// No secret manager.
  /// </summary>
  [EnumMember(Value = "none")]
  None,

  /// <summary>
  /// SOPS secret manager.
  /// </summary>
  [EnumMember(Value = "sops")]
  SOPS
}
