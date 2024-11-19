using System.CommandLine;

namespace KSail.Options;

sealed class LintOption() : Option<bool?>(
  ["--lint", "-l"],
  "Lint manifests before pushing an update"
)
{
}

