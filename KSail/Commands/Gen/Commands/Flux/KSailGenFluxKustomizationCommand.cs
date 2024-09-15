
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Commands.Gen.Options;

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
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        context.ExitCode = await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
