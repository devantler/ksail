using System.CommandLine;

namespace KSail.Options;

sealed class NameOption() : Option<string>(
  ["-n", "--name"],
  () => "ksail-default",
  "The name of the cluster."
)
{
}
