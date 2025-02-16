using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Commands.Secrets.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsImportCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _inputPathOption = new("Path to the input key file", ["--input", "-i"]) { Arity = ArgumentArity.ExactlyOne };
  internal KSailSecretsImportCommand() : base("import", "Import a key from a file")
  {
    AddOptions();
    AddValidator(commandResult =>
    {
      if (!commandResult.Children.Any(c => c.Symbol.Name == _inputPathOption.Name))
      {
        commandResult.ErrorMessage = $"✗ Option '{_inputPathOption.Name}' is required";
      }
    });
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        string inputPath = context.ParseResult.GetValueForOption(_inputPathOption) ?? throw new KSailException("input path is required");

        var cancellationToken = context.GetCancellationToken();
        KSailSecretsImportCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case Models.Project.KSailSecretManager.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case Models.Project.KSailSecretManager.SOPS:
            handler = new KSailSecretsImportCommandHandler(inputPath, new SOPSLocalAgeSecretManager());
            break;
        }
        Console.WriteLine($"🔑 Exporting '{config.Spec.Project.SecretManager}' key to '{inputPath}'");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (FileNotFoundException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_projectSecretManagerOption);
    AddOption(_inputPathOption);
  }
}
