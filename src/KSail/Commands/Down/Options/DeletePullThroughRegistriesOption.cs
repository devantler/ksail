using System.CommandLine;

namespace KSail.Commands.Down.Options;

internal class DeletePullThroughRegistriesOption() : Option<bool>(
  ["--delete-pull-through-registries", "-d"],
  () => false,
  "Delete pull through registries"
)
{
}
