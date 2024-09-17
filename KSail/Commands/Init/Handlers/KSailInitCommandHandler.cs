using Devantler.KubernetesGenerator.KSail.Models;
using KSail.Commands.Init.Enums;
using KSail.Commands.Init.Generators;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(string name, KSailKubernetesDistribution distribution, string outputPath, KSailInitTemplate template) : ICommandHandler
{
  readonly SOPSConfigFileGenerator _sopsConfigFileGenerator = new();
  readonly KSailClusterConfigGenerator _ksailClusterConfigGenerator = new();
  readonly DistributionConfigFileGenerator _distributionConfigFileGenerator = new();
  readonly ComponentsGenerator _componentsGenerator = new();
  readonly FluxSystemGenerator _fluxSystemGenerator = new();
  readonly VariablesGenerator _variablesGenerator = new();
  readonly InfrastructureGenerator _infrastructureGenerator = new();
  readonly CustomResourcesGenerator _customResourcesGenerator = new();
  readonly AppsGenerator _appsGenerator = new();

  public async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"📁 Initializing new cluster '{name}' in '{outputPath}' with the '{template}' template.");

    await _sopsConfigFileGenerator.GenerateAsync(name, outputPath, cancellationToken).ConfigureAwait(false);
    await _ksailClusterConfigGenerator.GenerateAsync(name, distribution, KSailGitOpsTool.Flux, outputPath, cancellationToken).ConfigureAwait(false);
    await _distributionConfigFileGenerator.GenerateAsync(name, distribution, outputPath, cancellationToken).ConfigureAwait(false);

    string k8sPath = Path.Combine(outputPath, "k8s");
    switch (template)
    {
      case KSailInitTemplate.K3dFluxDefault:
        await _componentsGenerator.GenerateAsync(k8sPath).ConfigureAwait(false);
        await _fluxSystemGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        await _variablesGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        await _infrastructureGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        await _customResourcesGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        await _appsGenerator.GenerateAsync(name, distribution, k8sPath, cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"The template '{template}' is not supported.");
    }

    return 0;
  }
}
