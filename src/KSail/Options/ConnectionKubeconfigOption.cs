using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// The kubeconfig to use for the connection.
/// </summary>
public class ConnectionKubeconfigOption() : Option<string>(
  ["-k", "--kubeconfig"],
  "Path to kubeconfig file"
)
{
}
