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

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);

      var token = context.GetCancellationToken();
      _ = await KSailStopCommandHandler.HandleAsync(clusterName, token);
    });
  }
}
