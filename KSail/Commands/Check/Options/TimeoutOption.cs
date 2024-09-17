using System.CommandLine;

namespace KSail.Commands.Check.Options;

sealed class TimeoutOption() : Option<int>(
  ["--timeout", "-t"],
  () => 600,
  "The time to wait for each kustomization to become ready."
)
{
}
