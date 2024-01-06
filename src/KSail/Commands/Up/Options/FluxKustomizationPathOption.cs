using System.CommandLine;

namespace KSail.Commands.Up.Options;

/// <summary>
/// The 'flux-kustomization-path' option responsible for specifying the path to the flux kustomization directory with -fkp or --flux-kustomization-path.
/// </summary>
/// <param name="name">The name of the cluster.</param>
public class FluxKustomizationPathOption(string name) : Option<string>(
  ["-fkp", "--flux-kustomization-path"],
  () => $"./clusters/{name}/flux",
  "path to the flux kustomization directory"
);
