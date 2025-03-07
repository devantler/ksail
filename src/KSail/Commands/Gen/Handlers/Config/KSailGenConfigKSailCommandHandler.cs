using KSail.Generator;
using KSail.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigKSailCommandHandler(string outputFile, bool overwrite)
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {
    var ksailCluster = new KSailCluster();
    await _ksailClusterGenerator.GenerateAsync(ksailCluster, outputFile, overwrite, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
