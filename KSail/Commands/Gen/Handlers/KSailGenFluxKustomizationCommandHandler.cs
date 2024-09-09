using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Sources;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers;

class KSailGenFluxKustomizationCommandHandler
{
  readonly FluxKustomizationGenerator _generator = new();
  public async Task<int> HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var fluxKustomization = new FluxKustomization
    {
      Metadata = new V1ObjectMeta
      {
        Name = "my-flux-kustomization"
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "10m",
        Path = "path/to/resources",
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxSourceType.OCIRepository,
          Name = "flux-system",
        }
      }
    };

    await _generator.GenerateAsync(fluxKustomization, outputPath, cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
