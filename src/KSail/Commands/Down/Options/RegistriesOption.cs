using System.CommandLine;

namespace KSail.Commands.Down.Options;

class RegistriesOption() : Option<bool?>(
  ["--registries", "-r"],
  "Delete registries"
)
{
}
