using System.CommandLine;
using KSail.Commands.SOPS.Options;
using KSail.Utils;

namespace KSail.Commands.SOPS.Commands;

sealed class KSailSOPSListCommand : Command
{
  readonly ShowPublicKeyOption _showPublicKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailSOPSListCommand() : base("list", "List keys")
  {
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.SOPSOptions.ListOptions.ShowPublicKey", context.ParseResult.GetValueForOption(_showPublicKeyOption));
        config.UpdateConfig("Spec.SOPSOptions.ListOptions.ShowPrivateKey", context.ParseResult.GetValueForOption(_showPrivateKeyOption));

        var cancellationToken = context.GetCancellationToken();
        //var handler = new KSailSOPSListCommandHandler(config);

        Console.WriteLine("🔑 Listing keys");
        //_ = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
