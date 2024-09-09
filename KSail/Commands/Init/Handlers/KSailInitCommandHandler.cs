using KSail.Commands.Init.Generators;
using KSail.Commands.Init.Models;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(string name, Distribution distribution, string outputPath, Template template) : ICommandHandler
{
  readonly SOPSConfigFileGenerator _sopsConfigFileGenerator = new();
  readonly DistributionConfigFileGenerator _distributionConfigFileGenerator = new();
  readonly ComponentsGenerator _componentsGenerator = new();
  readonly FluxSystemGenerator _fluxSystemGenerator = new();

  public async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"📁 Initializing new cluster '{name}' in '{outputPath}' with the '{template}' template.");
    await _sopsConfigFileGenerator.GenerateAsync(name, outputPath, cancellationToken).ConfigureAwait(false);
    await _distributionConfigFileGenerator.GenerateAsync(name, distribution, outputPath, cancellationToken).ConfigureAwait(false);

    string k8sPath = Path.Combine(outputPath, "k8s");
    switch (template)
    {
      case Template.KSail:
        await _componentsGenerator.GenerateAsync(k8sPath).ConfigureAwait(false);
        // TODO: Implement FluxSystemGenerator
        await _fluxSystemGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        // TODO: Implement VariablesGenerator
        await VariablesGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        // TODO: Implement InfrastructureGenerator
        await InfrastructureGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        // TODO: Implement CustomResourcesGenerator
        await CustomResourcesGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        // TODO: Implement AppsGenerator
        await AppsGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"The template '{template}' is not supported.");
    }

    return 0;
  }
}
