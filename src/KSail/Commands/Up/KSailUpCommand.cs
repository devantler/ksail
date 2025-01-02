using System.CommandLine;
using Devantler.FluxCLI;
using Devantler.K3dCLI;
using Devantler.KindCLI;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly EngineOption _engineOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _workingDirectoryOption = new("The directory in which to find the project") { Arity = ArgumentArity.ZeroOrOne };
  readonly ConfigOption _configOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SecretManagerOption _secretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DestroyOption _destroyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _reconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpCommand() : base("up", "Provision a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(context.ParseResult.GetValueForOption(_workingDirectoryOption), context.ParseResult.GetValueForOption(_nameOption), context.ParseResult.GetValueForOption(_distributionOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Connection.Timeout", context.ParseResult.GetValueForOption(_timeoutOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", context.ParseResult.GetValueForOption(_workingDirectoryOption));
        config.UpdateConfig("Spec.Project.ConfigPath", context.ParseResult.GetValueForOption(_configOption));
        config.UpdateConfig("Spec.Project.Engine", context.ParseResult.GetValueForOption(_engineOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_secretManagerOption));
        config.UpdateConfig("Spec.CLI.UpOptions.Destroy", context.ParseResult.GetValueForOption(_destroyOption));
        config.UpdateConfig("Spec.CLI.UpOptions.Lint", context.ParseResult.GetValueForOption(_lintOption));
        config.UpdateConfig("Spec.CLI.UpOptions.Reconcile", context.ParseResult.GetValueForOption(_reconcileOption));

        var handler = new KSailUpCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KSailException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KindException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (K3dException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (FluxException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_workingDirectoryOption);
    AddOption(_nameOption);
    AddOption(_engineOption);
    AddOption(_distributionOption);
    AddOption(_secretManagerOption);
    AddOption(_configOption);
    AddOption(_timeoutOption);
    AddOption(_destroyOption);
    AddOption(_lintOption);
    AddOption(_reconcileOption);
  }
}
