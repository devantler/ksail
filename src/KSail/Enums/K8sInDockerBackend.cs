namespace KSail.Enums;

/// <summary>
/// The k8s-in-docker backend.
/// </summary>
public enum K8sInDockerBackend
{
  /// <summary>
  /// The K3d k8s-in-docker backend.
  /// </summary>
  K3d = 0,
  /// <summary>
  /// The Talos k8s-in-docker backend.
  /// </summary>
  Talos = 1
}
