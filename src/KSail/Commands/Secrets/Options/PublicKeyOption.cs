using System.CommandLine;

namespace KSail.Commands.Secrets.Options;

class PublicKeyOption(string description) : Option<string?>(
  ["--public-key", "-pk"],
  description
)
{
}
