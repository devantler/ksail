using System.CommandLine;
using Devantler.SecretManager.SOPS.LocalAge;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Secrets.Commands;

sealed class KSailSecretsExportCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly PublicKeyArgument _publicKeyArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ProjectSecretManagerOption _projectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _outputPathOption = new("Path to the output file", ["--output", "-o"]) { Arity = ArgumentArity.ExactlyOne };
  internal KSailSecretsExportCommand() : base("export", "Export a key to a file")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.Project.SecretManager", context.ParseResult.GetValueForOption(_projectSecretManagerOption));
        string publicKey = context.ParseResult.GetValueForArgument(_publicKeyArgument);
        string outputPath = context.ParseResult.GetValueForOption(_outputPathOption) ?? "./key.txt";

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

  void AddOptions()
  {
    AddArgument(_publicKeyArgument);
    AddOption(_projectSecretManagerOption);
    AddOption(_outputPathOption);
  }
}
