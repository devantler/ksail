using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

class GitOpsToolOption() : Option<KSailGitOpsTool?>(
  ["-g", "--gitops-tool"],
  "The GitOps tool to use for the KSail cluster."
)
{
}
