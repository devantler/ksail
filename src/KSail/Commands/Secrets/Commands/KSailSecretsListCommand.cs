using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Commands.Secrets.Handlers;
using KSail.Models.Project.Enums;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsListCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  internal KSailSecretsListCommand() : base("list", "List keys")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadWithoptionsAsync(context);

        var cancellationToken = context.GetCancellationToken();
        KSailSecretsListCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case KSailSecretManagerType.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case KSailSecretManagerType.SOPS:
            handler = new KSailSecretsListCommandHandler(config, new SOPSLocalAgeSecretManager());
            break;
        }
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(CLIOptions.SecretManager.SOPS.ShowPrivateKeysInListingsOption);
    AddOption(CLIOptions.SecretManager.SOPS.ShowAllKeysInListingsOption);
  }
}
