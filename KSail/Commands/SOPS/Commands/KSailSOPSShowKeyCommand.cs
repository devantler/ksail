using System.CommandLine;
using KSail.Commands.SOPS.Options;
using KSail.Utils;

namespace KSail.Commands.SOPS.Commands;

sealed class KSailSOPSListCommand : Command
{
  readonly ShowPublicKeyOption _showPublicKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailSOPSListCommand() : base("list", "Manage secrets with SOPS")
  {
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.", context.ParseResult.GetValueForOption(context.ParseResult.GetValueForOption(_showPublicKeyOption)));

        var cancellationToken = context.GetCancellationToken();
        var handler = new KSailListCommandHandler(config);

        Console.WriteLine("📋 Listing clusters");
        _ = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }


      var cancellationToken = context.GetCancellationToken();
      try
      {
        var handler = new KSailSOPSCommandHandler();
        context.ExitCode = await handler.HandleAsync(clusterName, generateKey, showKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
