using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Stop.Handlers;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();

  internal KSailStopCommand() : base("stop", "Stop a K8s cluster")
  {
    AddArgument(_clusterNameArgument);

    this.SetHandler(KSailStopCommandHandler.HandleAsync, _clusterNameArgument);
  }
}
