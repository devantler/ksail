using System.CommandLine;

namespace KSail.Options;

sealed class MetadataNameOption() : Option<string>(
  ["-n", "--name"],
  "The name of the cluster."
)
{
}
