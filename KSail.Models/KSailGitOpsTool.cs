using System.Runtime.Serialization;

namespace KSail.Models;

/// <summary>
/// The GitOps tool to use for the KSail cluster.
/// </summary>
public enum KSailGitOpsTool
{
  /// <summary>
  /// The Flux GitOps tool.
  /// </summary>
  [EnumMember(Value = "flux")]
  Flux
}
