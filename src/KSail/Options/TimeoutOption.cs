using System.CommandLine;

namespace KSail.Options;

sealed class TimeoutOption() : Option<int>(
  ["--timeout", "-t"],
  () => 600,
  "The timeout in seconds to wait for each kustomization to become ready. Defaults to 600 seconds."
)
{
}
