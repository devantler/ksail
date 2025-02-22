using System.CommandLine;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly MetadataNameOption _metadataNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _projectWorkingDirectoryOption = new("The output directory", ["-o", "--output"]) { Arity = ArgumentArity.ZeroOrOne };
  readonly FluxDeploymentToolPostBuildVariablesOption _fluxDeploymentToolPostBuildVariablesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly KustomizeTemplateFlowsOption _kustomizeTemplateKustomizationsOption = new() { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
  readonly KustomizeTemplateHooksOption _kustomizeTemplateKustomizationHooksOption = new() { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
  readonly PathOption _projectConfigOption = new("The path to the ksail configuration file", ["--ksail-config", "-kc"]) { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectDeploymentToolOption _projectDeploymentToolOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectDistributionOption _projectDistributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectMirrorRegistriesOption _projectMirrorRegistriesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectTemplateOption _projectTemplateOption = new() { Arity = ArgumentArity.ZeroOrOne };

  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(
          context.ParseResult.GetValueForOption(_projectConfigOption),
          context.ParseResult.GetValueForOption(_projectWorkingDirectoryOption),
          context.ParseResult.GetValueForOption(_metadataNameOption),
          context.ParseResult.GetValueForOption(_projectDistributionOption)
        ).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_metadataNameOption));
        config.UpdateConfig("Spec.FluxDeploymentTool.PostBuildVariables", context.ParseResult.GetValueForOption(_fluxDeploymentToolPostBuildVariablesOption));
        config.UpdateConfig("Spec.KustomizeTemplate.Flows", context.ParseResult.GetValueForOption(_kustomizeTemplateKustomizationsOption));
        config.UpdateConfig("Spec.KustomizeTemplate.Hooks", context.ParseResult.GetValueForOption(_kustomizeTemplateKustomizationHooksOption));
        config.UpdateConfig("Spec.Project.DeploymentTool", context.ParseResult.GetValueForOption(_projectDeploymentToolOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_projectDistributionOption));
        config.UpdateConfig("Spec.Project.MirrorRegistries", context.ParseResult.GetValueForOption(_projectMirrorRegistriesOption));
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        config.UpdateConfig("Spec.Project.Template", context.ParseResult.GetValueForOption(_projectTemplateOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", context.ParseResult.GetValueForOption(_projectWorkingDirectoryOption));

        var handler = new KSailInitCommandHandler(config);
        Console.WriteLine($"📁 Initializing new cluster '{config.Metadata.Name}' in '{config.Spec.Project.WorkingDirectory}' with the '{config.Spec.Project.Template}' template.");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_metadataNameOption);
    AddOption(_projectWorkingDirectoryOption);
    AddOption(_projectConfigOption);
    AddOption(_projectDeploymentToolOption);
    AddOption(_projectDistributionOption);
    AddOption(_projectMirrorRegistriesOption);
    AddOption(_projectSecretManagerOption);
    AddOption(_projectTemplateOption);
    AddOption(_fluxDeploymentToolPostBuildVariablesOption);
    AddOption(_kustomizeTemplateKustomizationHooksOption);
    AddOption(_kustomizeTemplateKustomizationsOption);
  }
}
