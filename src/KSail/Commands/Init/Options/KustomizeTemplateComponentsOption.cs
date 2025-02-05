using System.CommandLine;

namespace KSail.Commands.Init.Options;

class KustomizeTemplateComponentsOption() : Option<bool>(
  ["-kc", "--kustomize-components"],
  "Generate components to reduce duplication."
)
{
}
