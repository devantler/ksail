using System.CommandLine;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Options;

class ProjectDeploymentToolOption(KSailCluster config) : Option<KSailDeploymentTool>(
  ["-dt", "--deployment-tool"],
  $"The Deployment tool to use for updating the state of the cluster. Default: '{config.Spec.Project.DeploymentTool}' (G)"
)
{
}
