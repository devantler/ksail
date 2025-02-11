using System.CommandLine;

namespace KSail.Options;

class ConnectionContextOption() : Option<string>(
  ["-c", "--context"],
  "The kubernetes context to use"
)
{
}
