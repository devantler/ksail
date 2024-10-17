using System.CommandLine;

namespace KSail.Commands.Init.Options;

class HelmReleasesOption() : Option<bool>(
  ["--helm-releases"],
  () => false,
  "Generate Helm releases for Traefik, Cert-Manager, and PodInfo."
)
{
}
