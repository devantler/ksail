using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Start.Handlers;

namespace KSail.Commands.Start;

internal sealed class KSailStartCommand : Command
{
  private readonly NameArgument nameArgument = new();

  internal KSailStartCommand() : base("start", "Start a K8s cluster")
  {
    AddArgument(nameArgument);

    this.SetHandler(KSailStartCommandHandler.HandleAsync, nameArgument);
  }
}
