namespace KSail.Models.DeploymentTool;

/// <summary>
/// Options for the KSail Deployment Tool.
/// </summary>
public class KSailDeploymentTool
{
  /// <summary>
  /// The Flux deployment tool.
  /// </summary>
  public KSailFluxDeploymentTool Flux { get; set; } = new KSailFluxDeploymentTool();
}
