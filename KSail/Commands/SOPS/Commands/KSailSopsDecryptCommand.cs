using System.CommandLine;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Sops.Commands;

sealed class KSailSopsDecryptCommand : Command
{
  readonly PathOption _pathOption = new("File to decrypt");
  internal KSailSopsDecryptCommand() : base("decrypt", "Decrypt a file")
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
