using KSail.Commands.Init.Generators.SubGenerators;

namespace KSail.Commands.Init.Generators;

class TemplateGenerator
{
  readonly FluxSystemGenerator _fluxSystemGenerator = new();
  readonly KustomizeFlowGenerator _kustomizeFlowGenerator = new();
  internal async Task GenerateAsync(TemplateGeneratorOptions options, CancellationToken cancellationToken = default)
  {
    await _fluxSystemGenerator.GenerateAsync(options, cancellationToken).ConfigureAwait(false);
    await _kustomizeFlowGenerator.GenerateAsync(options, cancellationToken).ConfigureAwait(false);
  }
}
