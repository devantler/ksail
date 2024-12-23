
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Flux;

sealed class KSailGenFluxKustomizationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./flux-kustomization.yaml");
  readonly KSailGenFluxKustomizationCommandHandler _handler = new();
  internal KSailGenFluxKustomizationCommand() : base("kustomization", "Generate a 'kustomize.toolkit.fluxcd.io/v1/Kustomization' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
          Console.WriteLine($"✚ generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
