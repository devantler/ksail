using System.CommandLine;

namespace KSail.Commands.Init.Options;

class ComponentsOption() : Option<bool>(
  ["--components"],
  () => false,
  "Generate components to reduce duplication."
)
{
}
