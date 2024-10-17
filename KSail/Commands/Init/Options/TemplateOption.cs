using System.CommandLine;
using Devantler.KubernetesGenerator.KSail.Models.Init;

namespace KSail.Commands.Init.Options;

class TemplateOption : Option<KSailInitTemplate>
{
  public TemplateOption() : base(
    ["-t", "--template"],
    () => KSailInitTemplate.FluxDefault,
    "The template to use for the initialized cluster.")
  {
  }
}
