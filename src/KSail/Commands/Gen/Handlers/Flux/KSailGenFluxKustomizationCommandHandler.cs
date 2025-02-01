using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using Devantler.KubernetesGenerator.Flux.Models.Kustomization;

namespace KSail.Commands.Gen.Handlers.Flux;

class KSailGenFluxKustomizationCommandHandler
{
  readonly FluxKustomizationGenerator _generator = new();
  public async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var fluxKustomization = new FluxKustomization
    {
      Metadata = new FluxNamespacedMetadata
      {
        Name = "flux-kustomization"
      },
      Spec = new FluxKustomizationSpec
      {
        Interval = "60m",
        Timeout = "3m",
        RetryInterval = "2m",
        DependsOn = [],
        SourceRef = new FluxKustomizationSpecSourceRef
        {
          Kind = FluxKustomizationSpecSourceRefKind.OCIRepository,
          Name = "flux-system",
        },
        Path = "path/to/kustomize-kustomization-dir",
        Prune = true,
        Wait = true

      }
    };

    await _generator.GenerateAsync(fluxKustomization, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
