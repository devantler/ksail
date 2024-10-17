using System.Runtime.Serialization;

namespace KSail.Models;

/// <summary>
/// The container engine to use for the KSail cluster.
/// </summary>
public enum KSailContainerEngine
{
  /// <summary>
  /// Docker container engine.
  /// </summary>
  [EnumMember(Value = "docker")]
  Docker
}
