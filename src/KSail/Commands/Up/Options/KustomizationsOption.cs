using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class KustomizationsOption() : Option<string>(
 ["--kustomizations", "-k"],
  "Path to the flux kustomization directory [default: ./k8s/clusters/<name>/flux-system]"
);
