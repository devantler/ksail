
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Commands.Gen.Options;
using KSail.Utils;

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
