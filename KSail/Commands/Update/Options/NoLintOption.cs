using System.CommandLine;

namespace KSail.Commands.Update.Options;

class NoLintOption : Option<bool>
{
  internal NoLintOption() : base(
    ["--no-lint", "-nl"],
    () => false,
    "Skip linting manifests"
  )
  {
  }
}
