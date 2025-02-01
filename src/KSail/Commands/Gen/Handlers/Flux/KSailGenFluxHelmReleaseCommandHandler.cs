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
        Name = "my-helm-release",
        Namespace = "my-namespace"
      },
      Spec = new FluxHelmReleaseSpec(new FluxHelmReleaseSpecChart
      {
        Spec = new FluxHelmReleaseSpecChartSpec
        {
          Chart = "my-chart",
          SourceRef = new FluxHelmReleaseSpecChartSpecSourceRef
          {
            Kind = FluxHelmReleaseSpecChartSpecSourceRefKind.HelmRepository,
            Name = "my-helm-repo"
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
