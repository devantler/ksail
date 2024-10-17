using System.CommandLine;
using KSail.Commands.Stop.Handlers;
using KSail.Options;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly NameOption _clusterNameArgument = new();

  internal KSailStopCommand() : base("stop", "Stop a cluster")
  {
    AddArgument(_clusterNameArgument);

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);

      var cancellationToken = context.GetCancellationToken();
      try
      {
        context.ExitCode = await KSailStopCommandHandler.HandleAsync(clusterName, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
