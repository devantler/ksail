using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Commands.Init.Options;

class TemplateOption() : Option<KSailProjectTemplate>
(
  ["-t", "--template"],
  "The template to use for the initialized cluster."
)
{
}
