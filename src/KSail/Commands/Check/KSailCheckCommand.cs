using System.CommandLine;
using System.Globalization;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Check.Options;
using KSail.Enums;
using KSail.Options;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
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
    this.SetHandler(async (kubernetesDistributionProvisioner, clusterName, kubeconfig, timeout) =>
    {
      var kubernetesDistributionType = await kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
      string context = $"{kubernetesDistributionType.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";
      var handler = new KSailCheckCommandHandler();
      await handler.HandleAsync(context, timeout, kubeconfig);
    }, _kubernetesDistributionProvisionerBinder, _clusterNameArgument, _kubeconfigOption, _timeoutOption);
  }
}
