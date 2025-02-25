using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Commands.Secrets.Arguments;
using KSail.Commands.Secrets.Handlers;
using KSail.Models.Project.Enums;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsDecryptCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly PathArgument _pathArgument = new("The path to the file to decrypt.") { Arity = ArgumentArity.ExactlyOne };
  readonly GenericPathOption _outputOption = new(string.Empty) { Arity = ArgumentArity.ZeroOrOne };

  internal KSailSecretsDecryptCommand() : base("decrypt", "Decrypt a file")
  {
    AddArgument(_pathArgument);
    AddOptions();
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadWithoptionsAsync(context);
        string path = context.ParseResult.GetValueForArgument(_pathArgument);
        string? output = context.ParseResult.GetValueForOption(_outputOption);
        var cancellationToken = context.GetCancellationToken();
        KSailSecretsDecryptCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case KSailSecretManagerType.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case KSailSecretManagerType.SOPS:
            handler = new KSailSecretsDecryptCommandHandler(config, path, output, new SOPSLocalAgeSecretManager());
            break;
        }
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
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
    AddOption(CLIOptions.SecretManager.SOPS.InPlaceOption);
    AddOption(_outputOption);
  }
}
