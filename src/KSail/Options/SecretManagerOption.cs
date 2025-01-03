using System.CommandLine;
using KSail.Models.Project;

namespace KSail.Options;

class SecretManagerOption() : Option<KSailSecretManager>(
  ["-sm", "--secret-manager"],
  "Configure which secret manager to use."
)
{
}
