using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

class ProjectSecretManagerOption() : Option<KSailSecretManager>(
  ["-sm", "--secret-manager"],
  "Configure which secret manager to use."
)
{
}
