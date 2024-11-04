using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Extensions;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ExactlyOne };
  readonly PathOption _manifestsPathOption = new("./k8s", "Path to the manifests directory") { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _noReconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
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
        var config = await KSailClusterConfigLoader.LoadAsync(context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsPathOption));
        config.UpdateConfig("Spec.UpdateOptions.Lint", context.ParseResult.GetValueForOption(_lintOption));
        config.UpdateConfig("Spec.UpdateOptions.Reconcile", context.ParseResult.GetValueForOption(_noReconcileOption));

        var handler = new KSailUpdateCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
    AddOption(_noReconcileOption);
  }
}
