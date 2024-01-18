using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Check.Handlers;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly NameArgument nameArgument = new();
  readonly TimeoutOption timeoutOption = new();

  internal KSailCheckCommand() : base("check", "Check the status of the cluster")
  {
    AddArgument(nameArgument);
    AddOption(timeoutOption);
    this.SetHandler((name, timeout) =>
      _ = KSailCheckCommandHandler.HandleAsync(
        name,
        timeout,
        new CancellationToken()
      ), nameArgument, timeoutOption
    );
  }
}
