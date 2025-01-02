using System.ComponentModel;

namespace KSail.Models.DeploymentTool;

/// <summary>
/// The options for the KSail Flux Deployment Tool.
/// </summary>
public class KSailFluxDeploymentToolOptions
{
  /// <summary>
  /// The source for reconciling GitOps resources.
  /// </summary>
  [Description("The source for reconciling GitOps resources.")]
  public IKSailGitOpsSource Source { get; set; } = new KSailOCIRepository();

  /// <summary>
  /// Enable Flux post-build variables.
  /// </summary>
  [Description("Enable Flux post-build variables.")]
  public bool PostBuildVariables { get; set; }
}
