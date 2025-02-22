using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// The name of the cluster.
/// </summary>
public class MetadataNameOption() : Option<string>(
  ["-n", "--name"],
  "The name of the cluster."
)
{
}
