using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class LintOption() : Option<bool>(
 ["--lint", "-l"],
  () => true,
  "Lint the manifests before applying them"
);
