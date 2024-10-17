using System.CommandLine;

namespace KSail.Options;

sealed class TimeoutOption() : Option<int>(
  ["-t", "--timeout"],
  () => 600,
  "The time to wait for each kustomization to become ready."
)
{
}
