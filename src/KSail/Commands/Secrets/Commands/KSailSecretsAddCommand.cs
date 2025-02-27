using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Commands.Secrets.Handlers;
using KSail.Models.Project.Enums;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsAddCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();

  internal KSailSecretsAddCommand() : base("add", "Add a new encryption key")
  {
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadWithoptionsAsync(context).ConfigureAwait(false);
        var cancellationToken = context.GetCancellationToken();
        KSailSecretsAddCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case KSailSecretManagerType.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case KSailSecretManagerType.SOPS:
            handler = new KSailSecretsAddCommandHandler(new SOPSLocalAgeSecretManager());
            break;
        }

        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}


