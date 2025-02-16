using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsExportCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly PublicKeyArgument _publicKeyArgument = new("The public key for the encryption key to export") { Arity = ArgumentArity.ExactlyOne };
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _outputFilePathOption = new("Path to the output file", ["--output", "-o"]) { Arity = ArgumentArity.ExactlyOne };
  internal KSailSecretsExportCommand() : base("export", "Export a key to a file")
  {
    AddArguments();
    AddOptions();

    AddValidator(commandResult =>
    {
      string? outputFilePath = commandResult.Children.FirstOrDefault(c => c.Symbol.Name == _outputFilePathOption.Name)?.Tokens[0].Value;
      if (!commandResult.Children.Any(c => c.Symbol.Name == _outputFilePathOption.Name))
      {
        commandResult.ErrorMessage = $"✗ Option '{_outputFilePathOption.Name}' is required";
      }
      else if (outputFilePath != null && Path.GetFileName(outputFilePath) == string.Empty)
      {
        commandResult.ErrorMessage = $"✗ '{outputFilePath}' is not a valid file path";
      }
    });
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        string publicKey = context.ParseResult.GetValueForArgument(_publicKeyArgument);
        string outputPath = context.ParseResult.GetValueForOption(_outputFilePathOption) ?? throw new KSailException("output path is required");

        var cancellationToken = context.GetCancellationToken();
        KSailSecretsExportCommandHandler handler;
        switch (config.Spec.Project.SecretManager)
        {
          default:
          case Models.Project.KSailSecretManager.None:
            _ = _exceptionHandler.HandleException(new KSailException("no secret manager configured"));
            context.ExitCode = 1;
            return;
          case Models.Project.KSailSecretManager.SOPS:
            handler = new KSailSecretsExportCommandHandler(publicKey, outputPath, new SOPSLocalAgeSecretManager());
            break;
        }
        Console.WriteLine($"🔑 Exporting '{config.Spec.Project.SecretManager}' key to '{outputPath}'");
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

  void AddArguments() => AddArgument(_publicKeyArgument);

  void AddOptions()
  {
    AddOption(_projectSecretManagerOption);
    AddOption(_outputFilePathOption);
  }
}
