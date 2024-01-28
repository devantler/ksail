using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Enums;
using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly ContainerEngineProvisionerBinder _containerEngineProvisionerBinder = new(ContainerEngineType.Docker);
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly DeletePullThroughRegistriesOption _deletePullThroughRegistriesOption = new();
  internal KSailDownCommand() : base("down", "Destroy a K8s cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_deletePullThroughRegistriesOption);

    this.SetHandler(async (containerEngineProvisioner, kubernetesDistributionProvisioner, nameArgument, deletePullThroughRegistriesOption) =>
    {
      var handler = new KSailDownCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner);
      await handler.HandleAsync(nameArgument, deletePullThroughRegistriesOption);
    }, _containerEngineProvisionerBinder, _kubernetesDistributionProvisionerBinder, _clusterNameArgument, _deletePullThroughRegistriesOption);
  }
}
