using System.CommandLine;
using KSail.Commands.Init.Enums;

namespace KSail.Commands.Init.Options;

class TemplateOption : Option<KSailInitTemplate>
{
  public TemplateOption() : base(
    ["-t", "--template"],
    () => KSailInitTemplate.K3dFluxDefault,
    "The template to use for the initialized cluster.")
  {
  }
}
