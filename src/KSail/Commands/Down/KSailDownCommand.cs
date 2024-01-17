using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly NameArgument nameArgument = new();
  readonly DeletePullThroughRegistriesOption deletePullThroughRegistriesOption = new();
  internal KSailDownCommand() : base("down", "Destroy a K8s cluster")
  {
    AddArgument(nameArgument);
    AddOption(deletePullThroughRegistriesOption);
    this.SetHandler(KSailDownCommandHandler.HandleAsync, nameArgument, deletePullThroughRegistriesOption);
  }
}
