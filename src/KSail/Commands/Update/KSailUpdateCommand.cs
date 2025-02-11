using System.CommandLine;
using Devantler.FluxCLI;
using KSail.Commands.Update.Handlers;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly MetadataNameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _workingDirectory = new("Path to the working directory for your project") { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _reconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ConnectionTimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpdateCommand() : base(
    "update",
    "Update a cluster"
  )
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(name: context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Connection.Timeout", context.ParseResult.GetValueForOption(_timeoutOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", context.ParseResult.GetValueForOption(_workingDirectory));
        config.UpdateConfig("Spec.CLI.Update.Lint", context.ParseResult.GetValueForOption(_lintOption));
        config.UpdateConfig("Spec.CLI.Update.Reconcile", context.ParseResult.GetValueForOption(_reconcileOption));

        var handler = new KSailUpdateCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
      }
      catch (YamlException ex)
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
    AddOption(_nameOption);
    AddOption(_workingDirectory);
    AddOption(_lintOption);
    AddOption(_reconcileOption);
    AddOption(_timeoutOption);
  }
}
