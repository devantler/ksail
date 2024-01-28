using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Provisioners.ContainerEngine;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly ContainerEngineProvisionerBinder containerEngineProvisionerBinder = new(ContainerEngine.Docker);
  readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly DeletePullThroughRegistriesOption deletePullThroughRegistriesOption = new();
  internal KSailDownCommand() : base("down", "Destroy a K8s cluster")
  {
    AddArgument(nameArgument);
    AddOption(deletePullThroughRegistriesOption);

    this.SetHandler(async (containerEngineProvisioner, nameArgument, deletePullThroughRegistriesOption) =>
    {
      var handler = new KSailDownCommandHandler(containerEngineProvisioner);
      await handler.HandleAsync(nameArgument, deletePullThroughRegistriesOption);
    }, containerEngineProvisionerBinder, nameArgument, deletePullThroughRegistriesOption);
  }
}
