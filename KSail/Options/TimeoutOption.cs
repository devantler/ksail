using System.CommandLine;

namespace KSail.Options;

sealed class TimeoutOption() : Option<string>(
  ["-t", "--timeout"],
  "The time to wait for each kustomization to become ready."
)
{
}
