using System.CommandLine;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Options.Project;

/// <summary>
/// The project template
/// </summary>
public class ProjectTemplateOption(KSailCluster config) : Option<KSailTemplateType>
(
  ["-t", "--template"],
  $"The template to use for the initialized cluster. [default: {config.Spec.Project.Template}]"
)
{
}
