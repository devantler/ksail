using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

/// <summary>
/// The project template
/// </summary>
public class ProjectTemplateOption() : Option<KSailProjectTemplate>
(
  ["-t", "--template"],
  "The template to use for the initialized cluster."
)
{
}
