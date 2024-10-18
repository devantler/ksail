using System.CommandLine;
using KSail.Models;

namespace KSail.Options;

class GitOpsToolOption() : Option<KSailGitOpsTool>(
  ["-g", "--gitops-tool"],
  () => KSailGitOpsTool.Flux,
  "The GitOps tool to use for the KSail cluster."
)
{
}
