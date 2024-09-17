using System.CommandLine;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Check.Options;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly KubeconfigOption _kubeconfigOption = new();
  readonly KubernetesContextOption _kubernetesContextOption = new();
  readonly TimeoutOption _timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of a cluster")
  {
    AddOption(_kubeconfigOption);
    AddOption(_kubernetesContextOption);
    AddOption(_timeoutOption);
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
      string kubeconfig = context.ParseResult.GetValueForOption(_kubeconfigOption) ??
        throw new InvalidOperationException("Kubeconfig not set");
      string? kubernetesContext = context.ParseResult.GetValueForOption(_kubernetesContextOption);
      int timeout = context.ParseResult.GetValueForOption(_timeoutOption);

      var cancellationToken = context.GetCancellationToken();
      var handler = new KSailCheckCommandHandler();
      try
      {
        context.ExitCode = await handler.HandleAsync(kubernetesContext, timeout, cancellationToken, kubeconfig).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
