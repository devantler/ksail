using System.CommandLine;

namespace KSail.Options;

sealed class ManifestsOption()
 : Option<string>(
    ["-m", "--manifests"],
    () => "./k8s",
    "Path to the manifests directory"
  )
{
}
