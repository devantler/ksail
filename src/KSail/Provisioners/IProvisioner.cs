using KSail.Enums;

namespace KSail.Provisioners;

/// <summary>
/// The interface for a provisioner.
/// </summary>
public interface IProvisioner
{
  /// <summary>
  /// Gets the provisioner for the specified k8s-in-docker backend.
  /// </summary>
  /// <param name="k8sInDockerBackend">The k8s-in-docker backend.</param>
  /// <returns>The provisioner.</returns>
  /// <exception cref="NotSupportedException"></exception>
  public static IProvisioner GetProvisioner(K8sInDockerBackend k8sInDockerBackend)
  => k8sInDockerBackend switch
  {
    K8sInDockerBackend.K3d => new K3dProvisioner(),
    K8sInDockerBackend.Talos => new TalosProvisioner(),
    _ => throw new NotSupportedException($"The '{k8sInDockerBackend}' k8s-in-docker backend is not supported.")
  };

  /// <summary>
  /// Creates a K8s cluster.
  /// </summary>
  /// <param name="name">The name of the cluster to create.</param>
  /// <param name="manifestsPath">The path to the manifests directory.</param>
  /// <param name="fluxKustomizationPath">The path to the Flux Kustomization files relative to the 'manifestsPath'.</param>
  /// <param name="configPath">The path to the cluster configuration file.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task CreateAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null);

  /// <summary>
  /// Destroys a K8s cluster.
  /// </summary>
  /// <param name="name">The name of the cluster to destroy.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task DestroyAsync(string name);
}
