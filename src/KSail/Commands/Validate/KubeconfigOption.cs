using System.CommandLine;

namespace KSail.Commands.Check.Options;

class KubeconfigOption() : Option<string>(
  ["--kubeconfig", "-k"],
  () => $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.kube/config",
  "Path to kubeconfig file"
)
{
}
