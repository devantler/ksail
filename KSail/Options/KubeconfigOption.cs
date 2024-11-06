using System.CommandLine;

namespace KSail.Options;

class KubeconfigOption() : Option<string>(
  ["-k", "--kubeconfig"],
  "Path to kubeconfig file"
)
{
}
