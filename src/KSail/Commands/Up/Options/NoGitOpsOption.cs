using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class NoGitOpsOption() : Option<bool>(
 ["--no-gitops", "-ng"],
  () => false,
  "Disable GitOps"
);
