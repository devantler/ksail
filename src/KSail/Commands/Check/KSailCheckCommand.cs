using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();
  readonly TimeoutOption _timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of the cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_timeoutOption);
    this.SetHandler((clusterName, timeout) =>
      _ = KSailCheckCommandHandler.HandleAsync(
        clusterName,
        timeout,
        new CancellationToken()
      ), _clusterNameArgument, _timeoutOption
    );
  }
}
