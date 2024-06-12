using System.CommandLine;

namespace KSail.Options;

class KubernetesContextOption() : Option<string>(
  ["--context", "-c"],
  "The Kubernetes context to use"
)
{
}
