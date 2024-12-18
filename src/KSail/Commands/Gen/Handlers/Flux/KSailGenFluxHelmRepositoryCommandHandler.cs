using Devantler.KubernetesGenerator.Flux;
using Devantler.KubernetesGenerator.Flux.Models;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Flux;

class KSailGenFluxHelmRepositoryCommandHandler
{
  readonly FluxHelmRepositoryGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var helmRepository = new FluxHelmRepository()
    {
      Metadata = new V1ObjectMeta
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>"
      },
      Spec = new FluxHelmRepositorySpec()
      {
        Url = new Uri("https://charts.example.com/charts")
      }
    };
    await _generator.GenerateAsync(helmRepository, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
