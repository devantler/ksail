using System.CommandLine;
using System.CommandLine.Invocation;
using Devantler.KubernetesGenerator.KSail.Models;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Deserializer;
using KSail.Options;
using KSail.Validators;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly NameOption _clusterNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly OutputOption _outputOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SOPSOption _sopsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly GitOpsToolOption _gitOpsToolOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ComponentsOption _componentsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly HelmReleasesOption _helmReleasesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TemplateOption _templateOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly KSailClusterDeserializer _kSailClusterConfigDeserializer = new();

  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    var config = _kSailClusterConfigDeserializer.LocateAndDeserializeAsync().Result;

    AddOptions();
    AddValidators(config);

    this.SetHandler(async (context) =>
    {
      await config.SetConfigValueAsync("metadata.name", context.ParseResult.GetValueForOption(_clusterNameOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.manifestsPath", context.ParseResult.GetValueForOption(_outputOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.sops", context.ParseResult.GetValueForOption(_sopsOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.distribution", context.ParseResult.GetValueForOption(_distributionOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.gitOpsTool", context.ParseResult.GetValueForOption(_gitOpsToolOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.initOptions.components", context.ParseResult.GetValueForOption(_componentsOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.initOptions.helmReleases", context.ParseResult.GetValueForOption(_helmReleasesOption)!).ConfigureAwait(false);
      await config.SetConfigValueAsync("spec.initOptions.template", context.ParseResult.GetValueForOption(_templateOption)!).ConfigureAwait(false);
      try
      {
        var handler = new KSailInitCommandHandler(CreateOptions(context, config));
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_clusterNameOption);
    AddOption(_componentsOption);
    AddOption(_distributionOption);
    AddOption(_helmReleasesOption);
    AddOption(_outputOption);
    AddOption(_sopsOption);
    AddOption(_templateOption);
  }

  void AddValidators(KSailCluster config)
  {
    AddValidator(new ClusterNameOptionValidator(config, _clusterNameOption).Validate);
    AddValidator(new DistributionOptionValidator(config, _distributionOption).Validate);
  }

  KSailInitCommandHandlerOptions CreateOptions(InvocationContext context, KSailCluster config)
  {
    return new KSailInitCommandHandlerOptions
    {
      ClusterName = config.Metadata.Name,
      Distribution = (KSailKubernetesDistribution)config.Spec?.Distribution!,
      OutputPath = context.ParseResult.GetValueForOption(_outputOption)!,
      Template = context.ParseResult.GetValueForOption(_templateOption),
      EnableSOPS = context.ParseResult.GetValueForOption(_sopsOption),
      IncludeComponents = context.ParseResult.GetValueForOption(_componentsOption),
      IncludeHelmReleases = context.ParseResult.GetValueForOption(_helmReleasesOption),
    };
  }
}
