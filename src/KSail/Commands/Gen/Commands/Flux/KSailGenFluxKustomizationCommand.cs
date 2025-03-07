
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Flux;

sealed class KSailGenFluxKustomizationCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./flux-kustomization.yaml");
  internal KSailGenFluxKustomizationCommand() : base("kustomization", "Generate a 'kustomize.toolkit.fluxcd.io/v1/Kustomization' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./flux-kustomization.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine($"✚ generating {outputFile}");
          var handler = new KSailGenFluxKustomizationCommandHandler(outputFile, overwrite);
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
