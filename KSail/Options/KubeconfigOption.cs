using System.CommandLine;

namespace KSail.Options;

class KubeconfigOption() : Option<string>(
  ["-k", "--kubeconfig"],
  () => $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.kube/config",
  "Path to kubeconfig file"
)
{
}
