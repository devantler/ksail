using System.ComponentModel;
using KSail.Models.Project;

namespace KSail.Models;

/// <summary>
/// A KSailCluster object to configure the KSail operator.
/// </summary>
public class KSailCluster
{
  /// <summary>
  /// The API version where the KSail Cluster object is defined.
  /// </summary>
  [Description("The API version where the KSail Cluster object is defined.")]
  public string ApiVersion { get; set; } = "ksail.io/v1alpha1";
  /// <summary>
  /// The KSail Cluster object kind.
  /// </summary>
  [Description("The KSail Cluster object kind.")]
  public string Kind { get; set; } = "Cluster";
  /// <summary>
  /// The metadata of the KSail Cluster object.
  /// </summary>
  [Description("The metadata of the KSail Cluster object.")]
  public KSailMetadata Metadata { get; set; } = new();
  /// <summary>
  /// The spec of the KSail Cluster object.
  /// </summary>
  [Description("The spec of the KSail Cluster object.")]
  public KSailClusterSpec Spec { get; set; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailCluster"/> class.
  /// </summary>
  public KSailCluster() =>
    Spec = new KSailClusterSpec(Metadata.Name);

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailCluster"/> class with the specified name.
  /// </summary>
  /// <param name="name"></param>
  public KSailCluster(string name)
  {
    Metadata.Name = name;
    Spec = new KSailClusterSpec(name);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailCluster"/> class with the specified distribution.
  /// </summary>
  /// <param name="distribution"></param>
  public KSailCluster(KSailKubernetesDistribution distribution) =>
    Spec = new KSailClusterSpec(Metadata.Name, distribution);

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailCluster"/> class with the specified name and distribution.
  /// </summary>
  /// <param name="name"></param>
  /// <param name="distribution"></param>
  public KSailCluster(string name, KSailKubernetesDistribution distribution)
  {
    Metadata.Name = name;
    Spec = new KSailClusterSpec(name, distribution);
  }
}
