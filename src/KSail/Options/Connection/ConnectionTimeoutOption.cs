using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Connection;


internal class ConnectionTimeoutOption(KSailCluster config) : Option<string>(
  ["-t", "--timeout"],
  $"The time to wait for each kustomization to become ready. [default: {config.Spec.Connection.Timeout}]"
)
{
}
