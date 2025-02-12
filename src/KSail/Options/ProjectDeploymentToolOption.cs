using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

class ProjectDeploymentToolOption() : Option<KSailDeploymentTool>(
  ["-dt", "--deployment-tool"],
  "The Deployment tool to use for updating the state of the cluster."
)
{
}
