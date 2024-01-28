using System.CommandLine;
using System.Globalization;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Enums;
using KSail.Options;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly ClusterNameArgument _clusterNameArgument = new();
  readonly TimeoutOption _timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of the cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_timeoutOption);
    this.SetHandler(async (kubernetesDistributionProvisioner, clusterName, timeout) =>
    {
      var kubernetesDistributionType = await kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
      string context = $"{kubernetesDistributionType.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";
      var handler = new KSailCheckCommandHandler();
      await handler.HandleAsync(context, timeout, new CancellationToken());
    }, _kubernetesDistributionProvisionerBinder, _clusterNameArgument, _timeoutOption);
  }
}
