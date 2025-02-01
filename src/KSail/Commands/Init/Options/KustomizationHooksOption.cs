using System.CommandLine;

namespace KSail.Commands.Init.Options;

class KustomizationHooksOption() : Option<IEnumerable<string>>
(
  ["-kh", "--kustomization-hooks"],
  "The kustomization hooks to include in the initialized cluster."
)
{
}
