using System.CommandLine;
using KSail.Models.Commands.Init;

namespace KSail.Commands.Init.Options;

class TemplateOption() : Option<KSailInitTemplate?>
(
  ["-t", "--template"],
  "The template to use for the initialized cluster."
)
{
}
