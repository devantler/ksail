using System.CommandLine;
using KSail.Commands.Down.Handlers;

namespace KSail.Commands.Down;

sealed class KSailDownK3dCommand : Command
{
  internal KSailDownK3dCommand(
   Option<string> nameOption
  ) : base("k3d", "destroy a K3d cluster ")
  {
    AddOption(nameOption);

    this.SetHandler(KSailDownK3dCommandHandler.HandleAsync, nameOption);
  }
}
