using System.CommandLine;
using KSail.Commands.Debug.Handlers;
using KSail.Options;

namespace KSail.Commands.Debug;

sealed class KSailDebugCommand : Command
{
  readonly KubernetesContextOption _kubernetesContextOption = new();
  readonly KubeconfigOption _kubeconfigOption = new() { IsRequired = true };

  internal KSailDebugCommand() : base("debug", "Debug the cluster")
  {
    AddOption(_kubernetesContextOption);
    AddOption(_kubeconfigOption);
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
      string? kubernetesContext = context.ParseResult.GetValueForOption(_kubernetesContextOption);
      string kubeconfig = context.ParseResult.GetValueForOption(_kubeconfigOption) ??
        throw new InvalidOperationException("Kubeconfig not set");

      var token = context.GetCancellationToken();
      var handler = new KSailDebugCommandHandler();
      try
      {
        context.ExitCode = await KSailDebugCommandHandler.HandleAsync(kubernetesContext, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
