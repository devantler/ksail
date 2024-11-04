
using Devantler.KubernetesGenerator.KSail;
using KSail.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigKSailCommandHandler
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var ksailCluster = new KSailCluster();
    await _ksailClusterGenerator.GenerateAsync(ksailCluster, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
