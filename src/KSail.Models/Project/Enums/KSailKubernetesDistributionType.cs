namespace KSail.Models.Project.Enums;

/// <summary>
/// The Kubernetes distribution to use for the KSail cluster.
/// </summary>
public enum KSailKubernetesDistributionType
{
  /// <summary>
  /// The kind Kubernetes distribution.
  /// </summary>
  Native,

  /// <summary>
  /// The k3s Kubernetes distribution.
  /// </summary>
  K3s

  ///// <summary>
  ///// The Talos Linux Kubernetes distribution.
  ///// </summary>
  // Talos
}
