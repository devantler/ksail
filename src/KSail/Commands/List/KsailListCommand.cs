using System.CommandLine;
using KSail.Commands.List.Handlers;

namespace KSail.Commands.List;

internal sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "List running clusters") =>
    this.SetHandler(KSailListCommandHandler.HandleAsync);
}
