using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

/// <summary>
/// The engine to use for provisioning the cluster.
/// </summary>
public class ProjectEngineOption()
 : Option<KSailEngine>(
    ["-e", "--engine"],
    "The engine to use for provisioning the cluster."
  )
{
}
