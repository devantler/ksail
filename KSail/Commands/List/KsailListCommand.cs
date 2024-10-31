using System.CommandLine;
using KSail.Commands.List.Handlers;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "List active clusters")
  {
    this.SetHandler(async (context) =>
    {
      var cancellationToken = context.GetCancellationToken();
      var handler = new KSailListCommandHandler();
      try
      {
        Console.WriteLine("📋 Listing active clusters");
        var clusters = await handler.HandleAsync(cancellationToken).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}