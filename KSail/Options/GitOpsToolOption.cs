using System.CommandLine;
using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Options;

class GitOpsToolOption() : Option<KSailGitOpsTool>(
  ["-g", "--gitops-tool"],
  () => KSailGitOpsTool.Flux,
  "The GitOps tool to use for the KSail cluster."
)
{
}
