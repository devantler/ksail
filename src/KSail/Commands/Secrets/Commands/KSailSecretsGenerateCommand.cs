using System.CommandLine;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsGenerateCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailSecretsGenerateCommand() : base("gen", "Generate a new encryption key")
  {
    AddOption(_projectSecretManagerOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        var cancellationToken = context.GetCancellationToken();
        var handler = new KSailSecretsGenerateCommandHandler();

        if (config.Spec.Project.SecretManager == Models.Project.KSailSecretManager.None)
        {
          _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
          context.ExitCode = 1;
          return;
        }

        Console.WriteLine($"🔑 Generating new encryption key with '{config.Spec.Project.SecretManager}'");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}


