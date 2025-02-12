using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Commands.Secrets.Handlers;
using KSail.Commands.Secrets.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsListCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly ShowPrivateKeysOption _showPrivateKeysOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowProjectKeysOption _showProjectKeysOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailSecretsListCommand() : base("list", "List keys")
  {
    AddOption(_showPrivateKeysOption);
    AddOption(_showProjectKeysOption);

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.CLI.Secrets.List.ShowPrivateKeys", context.ParseResult.GetValueForOption(_showPrivateKeysOption));
        config.UpdateConfig("Spec.CLI.Secrets.List.ShowProjectKeys", context.ParseResult.GetValueForOption(_showProjectKeysOption));

        var cancellationToken = context.GetCancellationToken();
        KSailSecretsListCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case Models.Project.KSailSecretManager.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case Models.Project.KSailSecretManager.SOPS:
            handler = new KSailSecretsListCommandHandler(config, new SOPSLocalAgeSecretManager());
            break;
        }
        Console.WriteLine("🔑 Listing keys");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine();
      }
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KSailException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
