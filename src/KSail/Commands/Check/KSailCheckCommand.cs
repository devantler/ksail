using System.CommandLine;
using KSail.Commands.Check.Handlers;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly NameOption nameOption = new("The name of the cluster to check.") { IsRequired = true };
  readonly TimeoutOption timeoutOption = new();
  readonly KSailCheckCommandHandler kSailCheckCommandHandler;

  internal KSailCheckCommand(IConsole console) : base("check", "Check the status of the cluster.")
  {
    kSailCheckCommandHandler = new(console);
    AddOption(nameOption);
    AddOption(timeoutOption);
    this.SetHandler((name, timeout) =>
      _ = kSailCheckCommandHandler.HandleAsync(
        name,
        timeout,
        new CancellationToken()
      ), nameOption, timeoutOption
    );
  }
}
