using System.CommandLine;

namespace KSail.Presentation.Commands.Up.Options;

/// <summary>
/// The <c>--pull-through-registries</c> option to enable pull-through registries.
/// </summary>
public class PullThroughRegistriesOption() : Option<bool>(
  ["-ptr", "--pull-through-registries"],
  () => true,
  "use pull-through registries"
);
