using System.CommandLine;

namespace KSail.Options;

class LintOption : Option<bool>
{
  internal LintOption() : base(
    ["--lint", "-l"],
    () => true,
    "Lint manifests before pushing an update"
  )
  {
  }
}
