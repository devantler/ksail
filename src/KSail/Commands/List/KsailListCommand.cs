using System.CommandLine;
using KSail.Commands.List.Handlers;
using KSail.Enums;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  internal KSailListCommand() : base("list", "List running clusters")
  {
    this.SetHandler(async (kubernetesDistributionProvisioner) =>
    {
      var handler = new KSailListCommandHandler(kubernetesDistributionProvisioner);
      _ = await handler.HandleAsync();
    }, _kubernetesDistributionProvisionerBinder);
  }
}
