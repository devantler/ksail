using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class FluxKustomizationPathOption(string name) : Option<string>(
 ["-fkp", "--flux-kustomization-path"],
  () => $"./clusters/{name}/flux",
  "path to the flux kustomization directory"
);
