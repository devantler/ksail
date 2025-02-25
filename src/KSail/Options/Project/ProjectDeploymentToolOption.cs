using System.CommandLine;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Options.Project;

/// <summary>
/// The Deployment tool to use for updating the state of the cluster.
/// </summary>
/// <param name="config"></param>
public class ProjectDeploymentToolOption(KSailCluster config) : Option<KSailDeploymentToolType>(
  ["-dt", "--deployment-tool"],
  $"The Deployment tool to use for updating the state of the cluster. [default: {config.Spec.Project.DeploymentTool}]"
)
{
}
