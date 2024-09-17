using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Flux;

class KSailGenFluxHelmReleaseCommandHandler
{
  readonly FluxHelmReleaseGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var helmRelease = new FluxHelmRelease()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>"
      },
      Spec = new FluxHelmReleaseSpec()
      {
        Interval = "10m",
        Chart = new FluxHelmReleaseSpecChart
        {
          Spec = new FluxHelmReleaseSpecChartSpec
          {
            Chart = "<chart>",
            SourceRef = new FluxSourceRef
            {
              Kind = FluxSource.HelmRepository,
              Name = "<name>"
            }
          }
        }
      }
    };
    await _generator.GenerateAsync(helmRelease, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
