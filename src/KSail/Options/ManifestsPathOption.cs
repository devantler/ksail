using System.CommandLine;

namespace KSail.Options;

sealed class ManifestsPathOption()
 : Option<string>(
    ["-mp", "--manifests-path"],
    () => "./k8s",
    "path to the manifests directory"
  )
{
}
