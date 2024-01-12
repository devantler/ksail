using System.CommandLine;

namespace KSail.Options;

sealed class ManifestsOption()
 : Option<string>(
    ["--manifests", "-m"],
    () => "./k8s",
    "path to the manifests directory"
  )
{
}
