using KSail.Commands.Init.Generators.SubGenerators;
using KSail.Models;

namespace KSail.Commands.Init.Generators;

class ProjectGenerator
{
  readonly FluxSystemGenerator _fluxSystemGenerator = new();
  readonly KustomizeFlowGenerator _kustomizeFlowGenerator = new();
  readonly ComponentsGenerator _componentsGenerator = new();
  readonly VariablesGenerator _variablesGenerator = new();
  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    await _fluxSystemGenerator.GenerateAsync(config, cancellationToken).ConfigureAwait(false);
    await _kustomizeFlowGenerator.GenerateAsync(config, cancellationToken).ConfigureAwait(false);
    if (config.Spec.CLI.InitOptions.Components)
      await _componentsGenerator.GenerateAsync(config, cancellationToken).ConfigureAwait(false);
    if (config.Spec.CLI.InitOptions.PostBuildVariables)
      await _variablesGenerator.GenerateAsync(config, cancellationToken).ConfigureAwait(false);
  }
}
