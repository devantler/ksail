using System.CommandLine;
using System.Globalization;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Check.Options;
using KSail.Options;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly KubeconfigOption _kubeconfigOption = new() { IsRequired = true };
  readonly TimeoutOption _timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of the cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_kubeconfigOption);
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
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      string kubeconfig = context.ParseResult.GetValueForOption(_kubeconfigOption) ??
        throw new InvalidOperationException("Kubeconfig not set");
      int timeout = context.ParseResult.GetValueForOption(_timeoutOption);

      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var kubernetesDistributionType = await kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
      string k8sContext = $"{kubernetesDistributionType.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";

      var token = context.GetCancellationToken();
      var handler = new KSailCheckCommandHandler();
      _ = await handler.HandleAsync(k8sContext, timeout, token, kubeconfig);
    });
  }
}
