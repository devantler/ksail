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

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);

      var token = context.GetCancellationToken();
      try
      {
        _ = await KSailStartCommandHandler.HandleAsync(clusterName, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
