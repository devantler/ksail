using System.CommandLine;
using KSail.Commands.Sops.Options;
using KSail.Utils;

namespace KSail.Commands.Sops.Commands;

sealed class KSailSopsListCommand : Command
{
  readonly ShowPublicKeyOption _showPublicKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailSopsListCommand() : base("list", "List keys")
  {
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.SopsOptions.ListOptions.ShowPublicKey", context.ParseResult.GetValueForOption(_showPublicKeyOption));
        config.UpdateConfig("Spec.SopsOptions.ListOptions.ShowPrivateKey", context.ParseResult.GetValueForOption(_showPrivateKeyOption));

        var cancellationToken = context.GetCancellationToken();
        //var handler = new KSailSopsListCommandHandler(config);

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
