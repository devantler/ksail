using System.CommandLine;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Sops.Commands;

sealed class KSailSopsEditCommand : Command
{
  readonly PathOption _pathOption = new("Path to the encrypted file");
  internal KSailSopsEditCommand() : base("edit", "Edit an encrypted file")
  {
    AddOption(_pathOption);

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
