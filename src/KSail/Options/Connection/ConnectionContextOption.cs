using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Connection;


internal class ConnectionContextOption(KSailCluster config) : Option<string>(
  ["-c", "--context"],
  $"The kubernetes context to use. [default: {config.Spec.Connection.Context}]"
)
{
}
