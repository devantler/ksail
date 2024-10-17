using System.CommandLine;
using KSail.Commands.Debug.Handlers;
using KSail.Commands.Debug.Options;
using KSail.Deserializer;
using KSail.Options;

namespace KSail.Commands.Debug;

sealed class KSailDebugCommand : Command
{
  readonly KSailClusterDeserializer _deserializer = new();
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
      var config = await _deserializer.LocateAndDeserializeAsync().ConfigureAwait(false);
      await config.SetConfigValueAsync("Spec.Kubeconfig", context.ParseResult.GetValueForOption(_kubeconfigOption)).ConfigureAwait(false);
      await config.SetConfigValueAsync("Spec.Context", context.ParseResult.GetValueForOption(_contextOption)).ConfigureAwait(false);
      await config.SetConfigValueAsync("Spec.DebugOptions.Editor", context.ParseResult.GetValueForOption(_editorOption)).ConfigureAwait(false);

      var handler = new KSailDebugCommandHandler(config);
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
