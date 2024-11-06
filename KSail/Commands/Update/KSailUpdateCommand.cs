using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _manifestsPathOption = new("Path to the manifests directory") { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _reconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
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
        config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsPathOption));
        config.UpdateConfig("Spec.Timeout", context.ParseResult.GetValueForOption(_timeoutOption));
        config.UpdateConfig("Spec.UpdateOptions.Lint", context.ParseResult.GetValueForOption(_lintOption));
        config.UpdateConfig("Spec.UpdateOptions.Reconcile", context.ParseResult.GetValueForOption(_reconcileOption));

        var handler = new KSailUpdateCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_nameOption);
    AddOption(_manifestsPathOption);
    AddOption(_lintOption);
    AddOption(_reconcileOption);
    AddOption(_timeoutOption);
  }
}
