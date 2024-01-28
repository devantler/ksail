using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly NameArgument _nameArgument = new();
  readonly TimeoutOption _timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of the cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_timeoutOption);
    this.SetHandler((name, timeout) =>
      _ = KSailCheckCommandHandler.HandleAsync(
        name,
        timeout,
        new CancellationToken()
      ), _nameArgument, _timeoutOption
    );
  }
}
