using System.CommandLine;

namespace KSail.Options;

sealed class ConnectionTimeoutOption() : Option<string>(
  ["-t", "--timeout"],
  "The time to wait for each kustomization to become ready."
)
{
}
