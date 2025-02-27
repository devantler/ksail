using KSail.Models;

namespace KSail.Options.DeploymentTool;



internal class DeploymentToolOptions(KSailCluster config)
{

  public DeploymentToolFluxOptions Flux { get; set; } = new(config);

}
