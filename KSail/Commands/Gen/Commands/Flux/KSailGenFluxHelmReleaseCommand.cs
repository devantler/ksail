
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Flux;

class KSailGenFluxHelmReleaseCommand : Command
{
  readonly FileOutputOption _outputOption = new("./helm-release.yaml");
  readonly KSailGenFluxHelmReleaseCommandHandler _handler = new();
  public KSailGenFluxHelmReleaseCommand() : base("helm-release", "Generate a 'helm.toolkit.fluxcd.io/v2/HelmRelease' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
