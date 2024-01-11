using System.CommandLine;
using KSail.Commands.Check.Handlers;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly NameOption nameOption = new("The name of the cluster to check.") { IsRequired = true };

  internal KSailCheckCommand() : base("check", "Check the status of the cluster.")
  {
    AddOption(nameOption);
    this.SetHandler((name) =>
      _ = KSailCheckHandler.HandleAsync(
        name,
        new CancellationToken()
      ), nameOption
    );
  }
}
