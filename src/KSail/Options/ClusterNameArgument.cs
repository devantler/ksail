using System.CommandLine;

namespace KSail.Options;

sealed class NameOption() : Option<string>(
  ["-n", "--name"],
  "The name of the cluster."
)
{
}
