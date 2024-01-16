using System.CommandLine;

namespace KSail.Commands.Up.Options;

internal sealed class KustomizationsOption() : Option<string>(
 ["--kustomizations", "-k"],
  "Path to the flux kustomization directory"
);
