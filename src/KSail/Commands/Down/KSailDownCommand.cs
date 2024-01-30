using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly DeletePullThroughRegistriesOption _deletePullThroughRegistriesOption = new();
  internal KSailDownCommand(CancellationToken token) : base("down", "Destroy a K8s cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_deletePullThroughRegistriesOption);
    this.SetHandler(async (context) =>
    {
      var containerEngineProvisioner = new DockerProvisioner();
      var kubernetesDistributionProvisioner = new K3dProvisioner();

      string nameArgument = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      bool deletePullThroughRegistriesOption = context.ParseResult.GetValueForOption(_deletePullThroughRegistriesOption);

      var handler = new KSailDownCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner);
      _ = await handler.HandleAsync(nameArgument, token, deletePullThroughRegistriesOption);
    });
  }
}
