using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The template to use for initializing a KSail cluster.
/// </summary>
public enum KSailProjectTemplate
{
  /// <summary>
  /// The kustomize template for a project using Kustomize to structure and customize the kubernetes manifests.
  /// </summary>
  [EnumMember(Value = "kustomize")]
  Kustomize
}
