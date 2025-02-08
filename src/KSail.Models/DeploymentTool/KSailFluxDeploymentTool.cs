using System.ComponentModel;

namespace KSail.Models.DeploymentTool;

/// <summary>
/// The options for the KSail Flux Deployment Tool.
/// </summary>
public class KSailFluxDeploymentTool
{
  /// <summary>
  /// The source for reconciling GitOps resources.
  /// </summary>
  [Description("The source for reconciling GitOps resources.")]
  public KSailRepository Source { get; set; } = new KSailRepository();

  /// <summary>
  /// Enable Flux post-build variables.
  /// </summary>
  [Description("Enable Flux post-build variables.")]
  public bool PostBuildVariables { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailFluxDeploymentTool"/> class.
  /// </summary>
  public KSailFluxDeploymentTool()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailFluxDeploymentTool"/> class with the specified GitOps source URL.
  /// </summary>
  /// <param name="gitOpsSourceUrl"></param>
  public KSailFluxDeploymentTool(Uri gitOpsSourceUrl) => Source = new KSailRepository(gitOpsSourceUrl);
}
