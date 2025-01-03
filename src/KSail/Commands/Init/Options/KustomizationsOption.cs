using System.CommandLine;

namespace KSail.Commands.Init.Options;

class KustomizationsOption() : Option<IEnumerable<string>>
(
  ["-k", "--kustomizations"],
  "The kustomizations to include in the initialized cluster."
)
{
}
