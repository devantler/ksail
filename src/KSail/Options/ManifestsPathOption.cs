using System.CommandLine;

namespace KSail.Options;

sealed class ManifestsPathOption() : Option<string>(
  ["--manifests-path", "-mp"],
  () => "./k8s",
  "path to the manifests directory"
)
{
}
