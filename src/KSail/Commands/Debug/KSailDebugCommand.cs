using System.CommandLine;
using KSail.Commands.Debug.Handlers;
using KSail.Commands.Debug.Options;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Debug;

sealed class KSailDebugCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly KubeconfigOption _kubeconfigOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ContextOption _contextOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly EditorOption _editorOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailDebugCommand() : base("debug", "Debug a cluster (❤️ K9s)")
  {
    AddOption(_kubeconfigOption);
    AddOption(_contextOption);
    AddOption(_editorOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Connection.Kubeconfig", context.ParseResult.GetValueForOption(_kubeconfigOption));
        config.UpdateConfig("Spec.Connection.Context", context.ParseResult.GetValueForOption(_contextOption));
        config.UpdateConfig("Spec.CLIOptions.DebugOptions.Editor", context.ParseResult.GetValueForOption(_editorOption));
        var handler = new KSailDebugCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine();
      }
      catch (YamlException ex)
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
}
