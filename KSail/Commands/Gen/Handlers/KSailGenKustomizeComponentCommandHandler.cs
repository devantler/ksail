using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;

namespace KSail.Commands.Gen.Handlers;

class KSailGenKustomizeComponentCommandHandler
{
  readonly KustomizeComponentGenerator _generator = new();
  public async Task<int> HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var kustomizeComponent = new KustomizeComponent();

    await _generator.GenerateAsync(kustomizeComponent, outputPath, cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
