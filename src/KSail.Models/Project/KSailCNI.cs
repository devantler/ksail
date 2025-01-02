using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The CNI to use.
/// </summary>
public enum KSailCNI
{
  /// <summary>
  /// The default CNI.
  /// </summary>
  [EnumMember(Value = "default")]
  Default,

  ///// <summary>
  ///// The Cilium CNI.
  ///// </summary>
  // [EnumMember(Value = "cilium")]
  // Cilium,

}
