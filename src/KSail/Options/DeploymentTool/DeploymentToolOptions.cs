using KSail.Models;

namespace KSail.Options.DeploymentTool;

/// <summary>
/// Options for the deployment tool.
/// </summary>
/// <param name="config"></param>
public class DeploymentToolOptions(KSailCluster config)
{
  /// <summary>
  /// Options for the Flux deployment tool.
  /// </summary>
  public DeploymentToolFluxOptions Flux { get; set; } = new(config);

}
