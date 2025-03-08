
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Flux;

class KSailGenFluxHelmRepositoryCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./helm-repository.yaml");
  public KSailGenFluxHelmRepositoryCommand() : base("helm-repository", "Generate a 'source.toolkit.fluxcd.io/v1/HelmRepository' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./helm-repository.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine(File.Exists(outputFile) ? (overwrite ?
            $"✚ overwriting '{outputFile}'" :
            $"✔ skipping '{outputFile}', as it already exists.") :
            $"✚ generating '{outputFile}'");
          var handler = new KSailGenFluxHelmRepositoryCommandHandler(outputFile, overwrite);
          context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
