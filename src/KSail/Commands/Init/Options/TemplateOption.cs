using System.CommandLine;
using KSail.Models.CLI.Commands.Init;

namespace KSail.Commands.Init.Options;

class TemplateOption() : Option<KSailCLIInitTemplate?>
(
  ["-t", "--template"],
  "The template to use for the initialized cluster."
)
{
}
