using System.CommandLine;

namespace KSail.Commands.Init.Options;

class KustomizeTemplateHooksOption() : Option<IEnumerable<string>>
(
  ["-kh", "--kustomize-hooks"],
  "The kustomize hooks to include in the initialized cluster."
)
{
}
