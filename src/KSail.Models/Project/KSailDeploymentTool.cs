using System.Runtime.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// The Deployment tool to use.
/// </summary>
public enum KSailDeploymentTool
{
  /// <summary>
  /// The Flux GitOps tool.
  /// </summary>
  [EnumMember(Value = "flux")]
  Flux

  ///// <summary>
  ///// The ArgoCD GitOps tool.
  ///// </summary>
  // [EnumMember(Value = "argocd")]
  // ArgoCD

  ///// <summary>
  ///// Kubectl apply.
  ///// </summary>
  // [EnumMember(Value = "kubectl")]
  // Kubectl
}
