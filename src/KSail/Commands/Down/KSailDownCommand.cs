using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;

namespace KSail.Commands.Down;

internal sealed class KSailDownCommand : Command
{
  private readonly NameArgument nameArgument = new();
  internal KSailDownCommand() : base("down", "Destroy a K8s cluster")
  {
    AddArgument(nameArgument);
    this.SetHandler(KSailDownCommandHandler.HandleAsync, nameArgument);
  }
}
