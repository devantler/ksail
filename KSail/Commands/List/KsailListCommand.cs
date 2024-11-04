using System.CommandLine;
using KSail.Commands.List.Handlers;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "List active clusters")
  {
    this.SetHandler(async (context) =>
    {
      try
      {
        var cancellationToken = context.GetCancellationToken();
        var handler = new KSailListCommandHandler();

        Console.WriteLine("📋 Listing active clusters");
        var clusters = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
