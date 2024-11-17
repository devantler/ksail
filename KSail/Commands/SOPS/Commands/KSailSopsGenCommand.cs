using System.CommandLine;
using KSail.Utils;

namespace KSail.Commands.Sops.Commands;

sealed class KSailSopsGenCommand : Command
{
  internal KSailSopsGenCommand() : base("gen", "Generate a key")
  {
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);

        var cancellationToken = context.GetCancellationToken();
        //var handler = new KSailSopsListCommandHandler(config);

        //_ = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
