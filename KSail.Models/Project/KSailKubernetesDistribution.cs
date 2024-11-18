using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The Kubernetes distribution to use for the KSail cluster.
/// </summary>
public enum KSailKubernetesDistribution
{
  /// <summary>
  /// The k3d Kubernetes distribution.
  /// </summary>
  [EnumMember(Value = "k3d")]
  K3d,

  /// <summary>
  /// The kind Kubernetes distribution.
  /// </summary>
  [EnumMember(Value = "kind")]
  Kind
}
