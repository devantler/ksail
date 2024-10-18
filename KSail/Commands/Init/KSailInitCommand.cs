using System.CommandLine;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Models;
using KSail.Options;
using KSail.Validators;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly OutputDirectoryOption _outputDirectoryOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SOPSOption _sopsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly GitOpsToolOption _gitOpsToolOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ComponentsOption _componentsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly HelmReleasesOption _helmReleasesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TemplateOption _templateOption = new() { Arity = ArgumentArity.ZeroOrOne };

  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    var config = KSailClusterConfigLoader.LoadAsync().Result;

    AddOptions();
    AddValidators(config);

    this.SetHandler(async (context) =>
    {
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
      config.UpdateConfig("Spec.Sops", context.ParseResult.GetValueForOption(_sopsOption));
      config.UpdateConfig("Spec.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
      config.UpdateConfig("Spec.GitOpsTool", context.ParseResult.GetValueForOption(_gitOpsToolOption));
      config.UpdateConfig("Spec.InitOptions.OutputDirectory", context.ParseResult.GetValueForOption(_outputDirectoryOption));
      config.UpdateConfig("Spec.InitOptions.Components", context.ParseResult.GetValueForOption(_componentsOption));
      config.UpdateConfig("Spec.InitOptions.HelmReleases", context.ParseResult.GetValueForOption(_helmReleasesOption));
      config.UpdateConfig("Spec.InitOptions.Template", context.ParseResult.GetValueForOption(_templateOption));
      try
      {
        var handler = new KSailInitCommandHandler(config);
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
    AddOption(_nameOption);
    AddOption(_componentsOption);
    AddOption(_distributionOption);
    AddOption(_helmReleasesOption);
    AddOption(_outputDirectoryOption);
    AddOption(_sopsOption);
    AddOption(_templateOption);
  }

  void AddValidators(KSailCluster config)
  {
    AddValidator(new NameOptionValidator(config, _nameOption).Validate);
    AddValidator(new DistributionOptionValidator(_distributionOption).Validate);

  }
}
