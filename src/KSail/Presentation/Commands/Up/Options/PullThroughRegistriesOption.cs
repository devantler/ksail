using System.CommandLine;

namespace KSail.Presentation.Commands.Up.Options;

sealed class PullThroughRegistriesOption() : Option<bool>(
 ["-ptr", "--pull-through-registries"],
  () => true,
  "use pull-through registries"
);
