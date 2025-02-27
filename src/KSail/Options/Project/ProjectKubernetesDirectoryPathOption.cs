using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Project;

internal class ProjectKubernetesDirectoryPathOption(KSailCluster config) : Option<string?>(
  ["--kubernetes-directory", "-kd"],
  $"The path to the kubernetes directory. [default: {config.Spec.Project.KubernetesDirectoryPath}]"
)
{
}
