using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Enums;
using KSail.Services.Provisioners.ContainerEngine;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly ContainerEngineProvisionerBinder _containerEngineProvisionerBinder = new(ContainerEngineType.Docker);
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly NameArgument _nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly DeletePullThroughRegistriesOption _deletePullThroughRegistriesOption = new();
  internal KSailDownCommand() : base("down", "Destroy a K8s cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_deletePullThroughRegistriesOption);

    this.SetHandler(async (containerEngineProvisioner, kubernetesDistributionProvisioner, nameArgument, deletePullThroughRegistriesOption) =>
    {
      var handler = new KSailDownCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner);
      await handler.HandleAsync(nameArgument, deletePullThroughRegistriesOption);
    }, _containerEngineProvisionerBinder, _kubernetesDistributionProvisionerBinder, _nameArgument, _deletePullThroughRegistriesOption);
  }
}
