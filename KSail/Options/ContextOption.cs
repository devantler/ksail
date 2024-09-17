using System.CommandLine;

namespace KSail.Options;

class KubernetesContextOption() : Option<string>(
  ["-c", "--context"],
  "The context to use"
)
{
}
