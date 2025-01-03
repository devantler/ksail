using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The Kubernetes distribution to use for the KSail cluster.
/// </summary>
public enum KSailKubernetesDistribution
{
  /// <summary>
  /// The kind Kubernetes distribution.
  /// </summary>
  [EnumMember(Value = "native")]
  Native,

  /// <summary>
  /// The k3s Kubernetes distribution.
  /// </summary>
  [EnumMember(Value = "k3s")]
  K3s

  ///// <summary>
  ///// The Talos Linux Kubernetes distribution.
  ///// </summary>
  // [EnumMember(Value = "talos")]
  // Talos
}
