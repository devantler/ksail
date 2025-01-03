using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The container engine to use for the KSail cluster.
/// </summary>
public enum KSailEngine
{
  /// <summary>
  /// Docker container engine.
  /// </summary>
  [EnumMember(Value = "docker")]
  Docker

  ///// <summary>
  ///// Podman container engine.
  ///// </summary>
  //[EnumMember(Value = "podman")]
  //Podman

  ///// <summary>
  ///// Cluster API created cluster.
  ///// </summary>
  // [EnumMember(Value = "cluster-api-<provider>")]
  // ClusterAPI
}
