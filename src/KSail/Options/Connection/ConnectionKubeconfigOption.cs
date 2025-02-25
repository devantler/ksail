using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Connection;

/// <summary>
/// The kubeconfig to use for the connection.
/// </summary>
public class ConnectionKubeconfigOption(KSailCluster config) : Option<string>(
  ["-k", "--kubeconfig"],
  $"Path to kubeconfig file. [default: {config.Spec.Connection.Kubeconfig}]"
)
{
}
