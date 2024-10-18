using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption _manifestsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ReconcileOption _noReconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpdateCommand() : base(
    "update",
    "Update a cluster"
  )
  {
    AddOption(_nameOption);
    AddOption(_manifestsOption);
    AddOption(_lintOption);
    AddOption(_noReconcileOption);
    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
      config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsOption));
      config.UpdateConfig("Spec.UpdateOptions.Lint", context.ParseResult.GetValueForOption(_lintOption));
      config.UpdateConfig("Spec.UpdateOptions.Reconcile", context.ParseResult.GetValueForOption(_noReconcileOption));

      var handler = new KSailUpdateCommandHandler(config);
      try
      {
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
