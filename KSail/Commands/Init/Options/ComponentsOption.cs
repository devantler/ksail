using System.CommandLine;

namespace KSail.Commands.Init.Options;

class ComponentsOption() : Option<bool>(
  ["-c", "--components"],
  () => false,
  "Generate components to reduce duplication."
)
{
}
