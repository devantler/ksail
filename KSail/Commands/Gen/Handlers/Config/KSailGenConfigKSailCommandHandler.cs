
using Devantler.KubernetesGenerator.KSail;
using KSail.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigKSailCommandHandler
{
  readonly KSailClusterGenerator _ksailClusterGenerator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var ksailCluster = new KSailCluster();
    await _ksailClusterGenerator.GenerateAsync(ksailCluster, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✚ Generating {outputPath}");
  }
}
