using System.CommandLine;
using k8s.Exceptions;
using KSail.Commands.Check.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly KubeconfigOption _kubeconfigOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ContextOption _contextOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailCheckCommand() : base("check", "Check the status of a cluster")
  {
    AddOptions();
    AddValidator(result =>
    {
      string? kubeconfigPath = result.GetValueForOption(_kubeconfigOption);
      if (string.IsNullOrWhiteSpace(kubeconfigPath) || !File.Exists(kubeconfigPath))
      {
        result.ErrorMessage = $"Kubeconfig file '{kubeconfigPath}' does not exist";
      }
    });

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Kubeconfig", context.ParseResult.GetValueForOption(_kubeconfigOption));
        string? kubeContext = context.ParseResult.GetValueForOption(_contextOption);
        if (kubeContext != null && !string.IsNullOrWhiteSpace(kubeContext) && !kubeContext.Equals("default", StringComparison.OrdinalIgnoreCase))
        {
          config.UpdateConfig("Spec.Context", context.ParseResult.GetValueForOption(_contextOption));
        }
        config.UpdateConfig("Spec.Timeout", context.ParseResult.GetValueForOption(_timeoutOption));

        Console.WriteLine("🔍 Checking cluster status");
        var handler = new KSailCheckCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine("");
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KubeConfigException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_kubeconfigOption);
    AddOption(_contextOption);
    AddOption(_timeoutOption);
  }
}
