using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Stop.Handlers;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();

  internal KSailStopCommand(CancellationToken token) : base("stop", "Stop a K8s cluster")
  {
    AddArgument(_clusterNameArgument);

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);

      _ = await KSailStopCommandHandler.HandleAsync(clusterName, token);
    });
  }
}
