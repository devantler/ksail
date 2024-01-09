using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class PullThroughRegistriesOption() : Option<bool>(
 ["--pull-through-registries", "-ptr"],
  () => true,
  "use pull-through registries"
);
