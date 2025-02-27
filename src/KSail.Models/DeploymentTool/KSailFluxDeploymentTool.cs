using System.ComponentModel;

namespace KSail.Models.DeploymentTool;


public class KSailFluxDeploymentTool
{

  [Description("The source for reconciling GitOps resources.")]
  public KSailRepository Source { get; set; } = new KSailRepository();

  public KSailFluxDeploymentTool()
  {
  }

  public KSailFluxDeploymentTool(Uri gitOpsSourceUrl) => Source = new KSailRepository(gitOpsSourceUrl);
}
