using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class FluxDeploymentToolSourceUrlOption() : Option<string>(
 ["--flux-source-url", "-fsu"],
  "Flux source URL for reconciling GitOps resources"
);
