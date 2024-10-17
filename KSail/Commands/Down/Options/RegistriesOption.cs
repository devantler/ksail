using System.CommandLine;

namespace KSail.Commands.Down.Options;

class RegistriesOption() : Option<bool>(
  ["--delete-pull-through-registries", "-d"],
  () => false,
  "Delete pull through registries"
)
{
}
