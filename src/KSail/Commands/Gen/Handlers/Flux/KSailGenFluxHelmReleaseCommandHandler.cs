using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.HelmRelease;

namespace KSail.Commands.Gen.Handlers.Flux;

class KSailGenFluxHelmReleaseCommandHandler
{
  readonly FluxHelmReleaseGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var helmRelease = new FluxHelmRelease()
    {
      Metadata = new FluxNamespacedMetadata
      {
        Name = "<name>",
        Namespace = "<namespace>"
      },
      Spec = new FluxHelmReleaseSpec(new FluxHelmReleaseSpecChart
      {
        Spec = new FluxHelmReleaseSpecChartSpec
        {
          Chart = "<chart>",
          SourceRef = new FluxSourceRef
          {
            Kind = FluxSourceRefKind.HelmRepository,
            Name = "<name>"
          }
        }
      })
      {
        Interval = "10m"
      }
    };
    await _generator.GenerateAsync(helmRelease, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
