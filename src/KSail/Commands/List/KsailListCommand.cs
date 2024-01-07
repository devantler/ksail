using System.CommandLine;
using KSail.Commands.List.Handlers;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "list running clusters") =>
    this.SetHandler(KSailListCommandHandler.Handle);
}
