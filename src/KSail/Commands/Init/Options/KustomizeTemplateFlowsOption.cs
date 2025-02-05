using System.CommandLine;

namespace KSail.Commands.Init.Options;

class KustomizeTemplateFlowsOption() : Option<IEnumerable<string>>
(
  ["-kf", "--kustomize-flows"],
  "The flows to include. The first depends on the next, and so on."
)
{
}
