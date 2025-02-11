using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

sealed class ProjectEngineOption()
 : Option<KSailEngine>(
    ["-e", "--engine"],
    "The engine to use for provisioning the cluster."
  )
{
}
