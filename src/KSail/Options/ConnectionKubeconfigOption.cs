using System.CommandLine;

namespace KSail.Options;

class ConnectionKubeconfigOption() : Option<string>(
  ["-k", "--kubeconfig"],
  "Path to kubeconfig file"
)
{
}
