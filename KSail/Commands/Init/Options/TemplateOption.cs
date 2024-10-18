using System.CommandLine;
using KSail.Models.Commands.Init;

namespace KSail.Commands.Init.Options;

class TemplateOption : Option<KSailInitTemplate>
{
  public TemplateOption() : base(
    ["-t", "--template"],
    () => KSailInitTemplate.Simple,
    "The template to use for the initialized cluster.")
  {
  }
}
