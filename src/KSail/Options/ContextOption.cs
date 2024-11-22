using System.CommandLine;

namespace KSail.Options;

class ContextOption() : Option<string>(
  ["-c", "--context"],
  "The kubernetes context to use"
)
{
}
