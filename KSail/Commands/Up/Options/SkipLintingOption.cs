using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class SkipLintingOption() : Option<bool>(
 ["--skip-linting", "-sl"],
  () => false,
  "Skip linting of manifests"
);
