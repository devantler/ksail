using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Stop.Handlers;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly NameArgument _nameArgument = new();

  internal KSailStopCommand() : base("stop", "Stop a K8s cluster")
  {
    AddArgument(_nameArgument);

    this.SetHandler(KSailStopCommandHandler.HandleAsync, _nameArgument);
  }
}
