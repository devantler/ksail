using System.CommandLine;
using KSail.Commands.Init.Models;

namespace KSail.Commands.Init.Options;

class TemplateOption : Option<Template>
{
  public TemplateOption() : base(
    ["-t", "--template"],
    () => Template.KSail,
    "The template to use for the initialized cluster.")
  {
  }
}
