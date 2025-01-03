using System.CommandLine;

namespace KSail.Commands.Init.Options;

class ComponentsOption() : Option<bool>(
  ["-c", "--components"],
  "Generate components to reduce duplication."
)
{
}
