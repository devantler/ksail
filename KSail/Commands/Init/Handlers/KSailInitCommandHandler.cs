using System.Globalization;
using Devantler.KubernetesGenerator.KSail.Models.Init;
using KSail.Commands.Init.Generators;
using KSail.Commands.Init.Generators.SubGenerators;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(KSailInitCommandHandlerOptions options)
{
  readonly SOPSConfigFileGenerator _sopsConfigFileGenerator = new();
  readonly KSailClusterConfigGenerator _ksailClusterConfigGenerator = new();
  readonly DistributionConfigFileGenerator _distributionConfigFileGenerator = new();
  readonly TemplateGenerator _templateGenerator = new();

  public async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"📁 Initializing new cluster '{options.ClusterName}' in '{options.OutputPath}' with the '{options.Template}' template.");

    await _ksailClusterConfigGenerator.GenerateAsync(
      options.ClusterName,
      options.Distribution,
      options.Template,
      options.OutputPath,
      cancellationToken
    ).ConfigureAwait(false);

    await _distributionConfigFileGenerator.GenerateAsync(
      options.ClusterName,
      options.Distribution,
      options.OutputPath,
      cancellationToken
    ).ConfigureAwait(false);

    if (options.EnableSOPS)
    {
      await _sopsConfigFileGenerator.GenerateAsync(
        options.ClusterName,
        options.OutputPath,
        cancellationToken
      ).ConfigureAwait(false);
    }

    switch (options.Template)
    {
      case KSailInitTemplate.FluxDefault:
        var templateOptions = new TemplateGeneratorOptions
        {
          ClusterName = options.ClusterName,
          Distribution = options.Distribution,
          KustomizeFlows = ["infrastructure", "apps"],
          KustomizeHooks = [""],
          EnableSOPS = options.EnableSOPS,
          IncludeComponents = options.IncludeComponents,
          IncludeVariables = options.IncludeVariables,
          IncludeHelmReleases = options.IncludeHelmReleases,
          OutputPath = Path.Combine(options.OutputPath, "k8s")
        };
        await _templateGenerator.GenerateAsync(templateOptions, cancellationToken).ConfigureAwait(false);
        break;
      case KSailInitTemplate.FluxAdvanced:
        templateOptions = new TemplateGeneratorOptions
        {
          ClusterName = options.ClusterName,
          Distribution = options.Distribution,
          KustomizeFlows = ["infrastructure/controllers", "infrastructure/configs", "apps"],
          KustomizeHooks = [$"clusters/{options.ClusterName}", $"distributions/{options.Distribution.ToString().ToLower(CultureInfo.CurrentCulture)}", "common"],
          EnableSOPS = options.EnableSOPS,
          IncludeComponents = options.IncludeComponents,
          IncludeVariables = options.IncludeVariables,
          IncludeHelmReleases = options.IncludeHelmReleases,
          OutputPath = Path.Combine(options.OutputPath, "k8s")
        };
        await _templateGenerator.GenerateAsync(templateOptions, cancellationToken).ConfigureAwait(false);
        break;
      default:
        throw new NotSupportedException($"The template '{options.Template}' is not supported.");
    }

    return 0;
  }
}
