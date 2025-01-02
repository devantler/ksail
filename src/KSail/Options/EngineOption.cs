using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

sealed class EngineOption()
 : Option<KSailEngine>(
    ["-e", "--engine"],
    "The engine to use for provisioning the cluster."
  )
{
}
