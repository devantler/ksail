using System.CommandLine;
using KSail.Commands.Start.Handlers;
using KSail.Options;

namespace KSail.Commands.Start;

sealed class KSailStartCommand : Command
{
  readonly NameOption _clusterNameArgument = new();

  internal KSailStartCommand() : base("start", "Start a cluster")
  {
    AddArgument(_clusterNameArgument);

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);

      var cancellationToken = context.GetCancellationToken();
      try
      {
        context.ExitCode = await KSailStartCommandHandler.HandleAsync(clusterName, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
