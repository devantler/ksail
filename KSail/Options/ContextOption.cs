using System.CommandLine;

namespace KSail.Options;

class ContextOption() : Option<string?>(
  ["-c", "--context"],
  () => null,
  "The kubernetes context to use"
)
{
}
