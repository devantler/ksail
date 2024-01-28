using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Start.Handlers;

namespace KSail.Commands.Start;

sealed class KSailStartCommand : Command
{
  readonly NameArgument _nameArgument = new();

  internal KSailStartCommand() : base("start", "Start a K8s cluster")
  {
    AddArgument(_nameArgument);

    this.SetHandler(KSailStartCommandHandler.HandleAsync, _nameArgument);
  }
}
