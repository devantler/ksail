using System.CommandLine;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _workingDirectoryOption = new("The directory in which to generate the project") { Arity = ArgumentArity.ZeroOrOne };
  readonly DeploymentToolOption _deploymentToolOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SecretManagerOption _secretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TemplateOption _templateOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly KustomizationsOption _kustomizationsOption = new() { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
  readonly KustomizationHooksOption _kustomizationHooksOption = new() { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
  readonly ComponentsOption _componentsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PostBuildVariablesOption _postBuildVariablesOption = new() { Arity = ArgumentArity.ZeroOrOne };

  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(name: context.ParseResult.GetValueForOption(_nameOption), distribution: context.ParseResult.GetValueForOption(_distributionOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", context.ParseResult.GetValueForOption(_workingDirectoryOption));
        config.UpdateConfig("Spec.Project.DeploymentTool", context.ParseResult.GetValueForOption(_deploymentToolOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_secretManagerOption));
        config.UpdateConfig("Spec.Project.Template", context.ParseResult.GetValueForOption(_templateOption));
        config.UpdateConfig("Spec.KustomizeTemplateOptions.Kustomizations", context.ParseResult.GetValueForOption(_kustomizationsOption));
        config.UpdateConfig("Spec.KustomizeTemplateOptions.KustomizationHooks", context.ParseResult.GetValueForOption(_kustomizationHooksOption));
        config.UpdateConfig("Spec.KustomizeTemplateOptions.Components", context.ParseResult.GetValueForOption(_componentsOption));
        config.UpdateConfig("Spec.FluxDeploymentToolOptions.PostBuildVariables", context.ParseResult.GetValueForOption(_postBuildVariablesOption));

        var handler = new KSailInitCommandHandler(config);
        Console.WriteLine($"📁 Initializing new cluster '{config.Metadata.Name}' in '{config.Spec.Project.WorkingDirectory}' with the '{config.Spec.Project.Template}' template.");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (NotSupportedException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (NullReferenceException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_nameOption);
    AddOption(_workingDirectoryOption);
    AddOption(_deploymentToolOption);
    AddOption(_distributionOption);
    AddOption(_secretManagerOption);
    AddOption(_templateOption);
    AddOption(_kustomizationsOption);
    AddOption(_kustomizationHooksOption);
    AddOption(_componentsOption);
    AddOption(_postBuildVariablesOption);
  }
}
