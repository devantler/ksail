using System.CommandLine;

namespace KSail.Options;

class ContextOption() : Option<string>(
  ["-c", "--context"],
  () => "default",
  "The kubernetes context to use"
)
{
}
