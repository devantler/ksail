using KSail.Enums;

namespace KSail.Provisioners.Cluster;

/// <summary>
/// The interface for a cluster provisioner.
/// </summary>
public interface IClusterProvisioner : IProvisioner
{
  /// <summary>
  /// Gets the provisioner for a specified k8s-in-docker backend.
  /// </summary>
  /// <param name="k8sInDockerBackend">The k8s-in-docker backend.</param>
  /// <returns>The provisioner.</returns>
  /// <exception cref="NotSupportedException"></exception>
  public static IClusterProvisioner GetProvisioner(K8sDistribution k8sInDockerBackend)
    => k8sInDockerBackend switch
    {
      K8sDistribution.K3d => new K3dProvisioner(),
      K8sDistribution.Talos => new TalosProvisioner(),
      _ => throw new NotSupportedException($"The '{k8sInDockerBackend}' k8s-in-docker backend is not supported.")
    };

  /// <summary>
  /// Creates a toolchain.
  /// </summary>
  /// <param name="name">The name of the cluster to create.</param>
  /// <param name="configPath">The path to the cluster configuration file.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task ProvisionAsync(string name, string? configPath = null);

  /// <summary>
  /// Deprovisions a toolchain.
  /// </summary>
  /// <param name="name">The name of the cluster to destroy.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task DeprovisionAsync(string name);
}
