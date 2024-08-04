using System.CommandLine;

namespace KSail.Options;

sealed class ManifestsOption()
 : Option<string>(
    ["--manifests", "-m"],
    () => "./k8s",
    "Path to the manifests directory"
  )
{
}
