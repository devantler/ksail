
using System.CommandLine;
using KSail.Commands.Gen.Handlers;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands;

sealed class KSailGenFluxKustomizationCommand : Command
{
  readonly FileOutputOption _outputOption = new() { IsRequired = true };
  readonly KSailGenFluxKustomizationCommandHandler _handler = new();
  internal KSailGenFluxKustomizationCommand() : base("flux-kustomization", "Generate a Flux Kustomization file.")
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
