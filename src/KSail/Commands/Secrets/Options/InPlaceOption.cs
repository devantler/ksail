using System.CommandLine;

namespace KSail.Commands.Secrets.Options;

class InPlaceOption(string description) : Option<bool>(
  ["--in-place", "-ip"],
  description
)
{
}
