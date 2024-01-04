using System.CommandLine;

namespace KSail.Presentation.Options;

/// <summary>
/// The 'manifests-path' option responsible for specifying the path to the manifests directory with -m or --manifests-path.
/// </summary>
public class ManifestsPathOption()
  : Option<string>(
    ["-mp", "--manifests-path"],
    () => "./k8s",
    "path to the manifests directory"
  )
{
}
