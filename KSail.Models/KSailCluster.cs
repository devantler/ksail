using k8s.Models;

namespace KSail.Models;

/// <summary>
/// A KSailCluster object to configure the KSail operator.
/// </summary>
public class KSailCluster
{
  /// <summary>
  /// The API version where the KSail Cluster object is defined.
  /// </summary>
  public string ApiVersion { get; } = "ksail.io/v1alpha1";
  /// <summary>
  /// The KSail Cluster object kind.
  /// </summary>
  public string Kind { get; } = "Cluster";
  /// <summary>
  /// The metadata of the KSail Cluster object.
  /// </summary>
  public V1ObjectMeta Metadata { get; set; } = new() { Name = "default" };
  /// <summary>
  /// The spec of the KSail Cluster object.
  /// </summary>
  public KSailClusterSpec Spec { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailCluster"/> class.
  /// </summary>
  public KSailCluster() =>
    Spec = new KSailClusterSpec(Metadata.Name);
}
