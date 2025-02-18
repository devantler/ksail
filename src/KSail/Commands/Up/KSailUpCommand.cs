using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly MetadataNameOption _metadataNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectDistributionOption _projectDistributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectEngineOption _projectEngineOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectMirrorRegistriesOption _projectMirrorRegistriesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly FluxDeploymentToolSourceUrlOption _fluxDeploymentToolSourceUrlOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _cliUpLintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _projectDistributionConfigOption = new("Path to the distribution configuration file", ["--distribution-config", "-dc"]) { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _projectWorkingDirectoryOption = new("The directory in which to find the project") { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _cliUpReconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ConnectionTimeoutOption _connectionTimeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpCommand() : base("up", "Create a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(context.ParseResult.GetValueForOption(_projectWorkingDirectoryOption), context.ParseResult.GetValueForOption(_metadataNameOption), context.ParseResult.GetValueForOption(_projectDistributionOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_metadataNameOption));
        config.UpdateConfig("Spec.Connection.Timeout", context.ParseResult.GetValueForOption(_connectionTimeoutOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", context.ParseResult.GetValueForOption(_projectWorkingDirectoryOption));
        config.UpdateConfig("Spec.Project.DistributionConfigPath", context.ParseResult.GetValueForOption(_projectDistributionConfigOption));
        config.UpdateConfig("Spec.Project.Engine", context.ParseResult.GetValueForOption(_projectEngineOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_projectDistributionOption));
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        config.UpdateConfig("Spec.Project.MirrorRegistries", context.ParseResult.GetValueForOption(_projectMirrorRegistriesOption));
        config.UpdateConfig("Spec.KustomizeTemplate.Root", $"k8s/clusters/{config.Metadata.Name}/flux-system");
        config.UpdateConfig("Spec.FluxDeploymentTool.Source.Url", context.ParseResult.GetValueForOption(_fluxDeploymentToolSourceUrlOption));
        config.UpdateConfig("Spec.CLI.Up.Lint", context.ParseResult.GetValueForOption(_cliUpLintOption));
        config.UpdateConfig("Spec.CLI.Up.Reconcile", context.ParseResult.GetValueForOption(_cliUpReconcileOption));

        var handler = new KSailUpCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
    AddOption(_connectionTimeoutOption);
    AddOption(_projectDistributionConfigOption);
    AddOption(_projectDistributionOption);
    AddOption(_projectEngineOption);
    AddOption(_projectMirrorRegistriesOption);
    AddOption(_projectSecretManagerOption);
    AddOption(_projectWorkingDirectoryOption);
    AddOption(_fluxDeploymentToolSourceUrlOption);
    AddOption(_cliUpLintOption);
    AddOption(_cliUpReconcileOption);
  }
}
