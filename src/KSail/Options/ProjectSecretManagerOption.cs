using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

/// <summary>
/// The secret manager to use for the cluster.
/// </summary>
public class ProjectSecretManagerOption() : Option<KSailSecretManager>(
  ["-sm", "--secret-manager"],
  "Configure which secret manager to use."
)
{
}
