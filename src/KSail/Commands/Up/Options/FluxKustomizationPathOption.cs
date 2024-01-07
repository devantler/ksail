using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class FluxKustomizationPathOption() : Option<string>(
 ["-fkp", "--flux-kustomization-path"],
  "path to the flux kustomization directory"
);
