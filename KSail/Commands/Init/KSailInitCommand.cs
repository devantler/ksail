using System.CommandLine;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly ComponentsOption _componentsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DeclarativeConfigOption _declarativeConfigOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly GitOpsToolOption _gitOpsToolOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly HelmReleasesOption _helmReleasesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly OutputDirectoryOption _outputDirectoryOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SOPSOption _sopsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PostBuildVariablesOption _postBuildVariablesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TemplateOption _templateOption = new() { Arity = ArgumentArity.ZeroOrOne };

  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(name: context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Project.Sops", context.ParseResult.GetValueForOption(_sopsOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
        config.UpdateConfig("Spec.Project.GitOpsTool", context.ParseResult.GetValueForOption(_gitOpsToolOption));
        config.UpdateConfig("Spec.CLI.InitOptions.OutputDirectory", context.ParseResult.GetValueForOption(_outputDirectoryOption));
        config.UpdateConfig("Spec.CLI.InitOptions.DeclarativeConfig", context.ParseResult.GetValueForOption(_declarativeConfigOption));
        config.UpdateConfig("Spec.CLI.InitOptions.PostBuildVariables", context.ParseResult.GetValueForOption(_postBuildVariablesOption));
        config.UpdateConfig("Spec.CLI.InitOptions.Components", context.ParseResult.GetValueForOption(_componentsOption));
        config.UpdateConfig("Spec.CLI.InitOptions.HelmReleases", context.ParseResult.GetValueForOption(_helmReleasesOption));
        config.UpdateConfig("Spec.CLI.InitOptions.Template", context.ParseResult.GetValueForOption(_templateOption));

        var handler = new KSailInitCommandHandler(config);
        Console.WriteLine($"📁 Initializing new cluster '{config.Metadata.Name}' in '{config.Spec.CLI.InitOptions.OutputDirectory}' with the '{config.Spec.CLI.InitOptions.Template}' template.");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (NullReferenceException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_nameOption);
    AddOption(_declarativeConfigOption);
    AddOption(_postBuildVariablesOption);
    AddOption(_componentsOption);
    AddOption(_distributionOption);
    AddOption(_helmReleasesOption);
    AddOption(_outputDirectoryOption);
    AddOption(_sopsOption);
    AddOption(_templateOption);
  }
}
