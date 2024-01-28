using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Start.Handlers;

namespace KSail.Commands.Start;

sealed class KSailStartCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();

  internal KSailStartCommand() : base("start", "Start a K8s cluster")
  {
    AddArgument(_clusterNameArgument);

    this.SetHandler(KSailStartCommandHandler.HandleAsync, _clusterNameArgument);
  }
}
