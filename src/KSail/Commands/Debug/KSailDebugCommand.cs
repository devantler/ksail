using System.CommandLine;
using KSail.Commands.Debug.Handlers;
using KSail.Options;

namespace KSail.Commands.Debug;

sealed class KSailDebugCommand : Command
{
  readonly KubeconfigOption _kubeconfigOption = new() { IsRequired = true };
  readonly KubernetesContextOption _kubernetesContextOption = new();

  internal KSailDebugCommand() : base("debug", "Debug the cluster (❤️ to K9s)")
  {
    AddOption(_kubeconfigOption);
    AddOption(_kubernetesContextOption);
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

      var token = context.GetCancellationToken();
      var handler = new KSailDebugCommandHandler();
      try
      {
        context.ExitCode = await KSailDebugCommandHandler.HandleAsync(kubeconfig, kubernetesContext, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
