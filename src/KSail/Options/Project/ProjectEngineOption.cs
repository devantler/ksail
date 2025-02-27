using System.CommandLine;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Options.Project;


internal class ProjectEngineOption(KSailCluster config)
 : Option<KSailEngineType>(
    ["-e", "--engine"],
    $"The engine to use for provisioning the cluster. [default: {config.Spec.Project.Engine}]"
  )
{
}
