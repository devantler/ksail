using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Stop.Handlers;

namespace KSail.Commands.Stop;

internal sealed class KSailStopCommand : Command
{
  private readonly NameArgument nameArgument = new();

  internal KSailStopCommand() : base("stop", "Stop a K8s cluster")
  {
    AddArgument(nameArgument);

    this.SetHandler(KSailStopCommandHandler.HandleAsync, nameArgument);
  }
}
