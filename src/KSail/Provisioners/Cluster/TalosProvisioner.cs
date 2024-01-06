namespace KSail.Provisioners.Cluster;

/// <summary>
/// A provisioner for provisioning Talos clusters.
/// </summary>
public class TalosProvisioner : IClusterProvisioner
{
  /// <summary>
  /// Creates a Talos cluster.
  /// </summary>
  /// <param name="name">The name of the cluster to create.</param>
  /// <param name="manifestsPath">The path to the manifests directory.</param>
  /// <param name="fluxKustomizationPath">The path to the Flux Kustomization files relative to the 'manifestsPath'.</param>
  /// <param name="configPath">The path to the cluster configuration file.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  /// <exception cref="NotImplementedException"></exception>
  public Task ProvisionAsync(string name, string manifestsPath, string fluxKustomizationPath, string? configPath = null) => throw new NotImplementedException();

  /// <summary>
  /// Destroys a Talos cluster.
  /// </summary>
  /// <param name="name">The name of the cluster to destroy.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  /// <exception cref="NotImplementedException"></exception>
  public Task DeprovisionAsync(string name) => throw new NotImplementedException();
}
