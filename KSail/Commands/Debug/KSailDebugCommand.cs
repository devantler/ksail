using System.CommandLine;
using KSail.Commands.Debug.Handlers;
using KSail.Commands.Debug.Options;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Debug;

sealed class KSailDebugCommand : Command
{
  readonly KubeconfigOption _kubeconfigOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ContextOption _contextOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly EditorOption _editorOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailDebugCommand() : base("debug", "Debug a cluster (❤️ K9s)")
  {
    AddOption(_kubeconfigOption);
    AddOption(_contextOption);
    AddOption(_editorOption);
    AddValidator(result =>
    {
      string? kubeconfig = result.GetValueForOption(_kubeconfigOption);
      if (string.IsNullOrWhiteSpace(kubeconfig) || !File.Exists(kubeconfig))
      {
        result.ErrorMessage = $"Kubeconfig file '{kubeconfig}' does not exist";
      }
    });
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Kubeconfig", context.ParseResult.GetValueForOption(_kubeconfigOption));
        config.UpdateConfig("Spec.Context", context.ParseResult.GetValueForOption(_contextOption));
        config.UpdateConfig("Spec.DebugOptions.Editor", context.ParseResult.GetValueForOption(_editorOption));

        var handler = new KSailDebugCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine("");
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
